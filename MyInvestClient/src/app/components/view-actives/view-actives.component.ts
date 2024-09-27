import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActiveService } from '../../services/active.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { LoadingComponent } from '../layout/loading/loading.component';
import { AuthService } from '../../services/auth.service';

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
  selector: 'app-view-actives',
  standalone: true,
  imports: [CommonModule, LoadingComponent],
  templateUrl: './view-actives.component.html',
  styleUrl: './view-actives.component.scss'
})
export class ViewActivesComponent implements OnInit{
  purseId: string = '';
  userId: string = '';
  actives: Active[] = [];
  activesQty: number = 0;
  selectedMenu: string = 'RESUMO';

  @ViewChild('actions_menu', { static: false }) actions_menu!: ElementRef;
  @ViewChild('actions', { static: false }) actions!: ElementRef;

  isLoading: boolean = true;

  constructor(
    private activeService: ActiveService,
    private activedRoute: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getId();

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
        if (response.status === 200)
        {
          this.populateTheArrayOfActives(response.body);
        }
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 404)
        {
          return;
        }
        if (err.status === 500)
        {
          if (typeof window !== 'undefined')
          {
            alert("Carteira criada com sucesso!");
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

  ngAfterViewInit(): void {
    this.actions_menu.nativeElement.addEventListener('click', () => {
      this.actions.nativeElement.classList.add('active');
    })
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
    this.router.navigate(["/"]);
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

        if (this.actives.length == 0) {

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

  backToPurses(): void {
    this.router.navigate(["/purses"])
  }
}
