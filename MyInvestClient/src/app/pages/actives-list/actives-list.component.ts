import { HttpResponse } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { ActiveService } from '@services/active.service';
import { AppService } from '@services/app.service';

interface Active {
  id: string
  ativo: string,
  tipo: string,
  dividentYield: string,
  precoAtual: string,
  preco_Teto: string,
  indicacao: string,
  proventos_pagos: string
}

@Component({
  selector: 'app-actives-list',
  templateUrl: './actives-list.component.html',
  styleUrl: './actives-list.component.scss'
})
export class ActivesListComponent implements OnInit{
  purseId: string = '';
  userId: string = '';
  actives: Active[] = [];
  activesQty: number = 0;
  selectedMenu: string = 'RESUMO';
  redirectBackLink: string = '/purses';
  faEdit = faEdit;
  faTrash = faTrash;

  isLoading: boolean = true;

  constructor(
    private activeService: ActiveService,
    private activedRoute: ActivatedRoute,
    private appService: AppService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userId = this.appService.getId();

    var param = this.activedRoute.snapshot.paramMap.get('purse');
    if (param != null)
    {
      this.purseId = param; 
    } 
    else 
    {
      alert("Ocorreu um erro ao tentar buscar os ativos!");
      return;
    }

    this.activeService.searchActivesForShowPurseDetails(param).subscribe({
      next: (response: HttpResponse<any>) => {
        console.log(response);
        if (response.status === 200)
        {
          this.populateTheArrayOfActives(response.body);
        }
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 401)
        {
          this.appService.redirectAfterExpiredAccessToken();
          return;
        }

        if (err.status === 404)
        {
          return;
        }
        
        if (err.status === 500)
        {
          if (typeof window !== 'undefined')
          {
            alert("Houve um erro ao tentar buscar os ativos!");
          }
        }
        console.log(err);
      }
    })
  }

  selectOptionMenu(optionMenu: string): void
  {
    this.selectedMenu = optionMenu;
  }

  populateTheArrayOfActives(body: any): void
  {
    if (body.length > 0)
    {
      this.actives = body.map((active: any) => {
        return {
          id: active.id,
          ativo: active.ativo,
          tipo: active.tipo,
          dividentYield: active.dividentYield,
          precoAtual: active.precoAtual,
          preco_Teto: active.preco_Teto,
          indicacao: active.indicacao,
          proventos_pagos: active.proventos_pagos
        }
      });
      this.activesQty = body.length;
      this.isLoading = false;
      console.log(this.actives);
    }
    else
    {
      this.isLoading = false;
    }
  }

  redirectToActive(code: string, dyDesiredPercentage: string): void
  {
    this.router.navigate([`/view-ticker/${code}/${dyDesiredPercentage}`]);
  }

  createActive(): void 
  {
    // this.router.navigate(["/"]);
  }

  deleteActive(purseId: any): void 
  {
    this.isLoading = true;
    this.activeService.delete(purseId).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 204)
        {
          this.actives = this.actives.filter(active => active.id !== purseId);
          this.isLoading = false;
        }
      },
      error: (err) => {
        if (typeof window !== 'undefined')
        {
          alert("Ocorreu um erro ao tentar deletar o ativo!");
          this.isLoading = false;
          console.log(`Ocorreu um erro ao tentar deletar o ativo! err: ${err.message}`);
          return;
        }
      }
    })
  }

  updateActive(activeId: string, activeCode: string, percentValue: string)
  {
    this.router.navigate(["/edit-ticker/" + activeId + "/" +  activeCode + "/" + percentValue])
  }

}
