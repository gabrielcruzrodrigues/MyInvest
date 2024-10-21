import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { AppService } from '@services/app.service';
import { PursesService } from '@services/purses.service';
import { UserService } from '@services/user.service';

interface Purse {
  purse_Id: number;
  name: string;
  description: string;
  createdAt: string;
}

@Component({
  selector: 'app-purses-list',
  templateUrl: './purses-list.component.html',
  styleUrl: './purses-list.component.scss'
})
export class PursesListComponent implements OnInit{

  userId: string = '';
  purses: Purse[] = [];
  isLoading: boolean = true;
  pursesQty: number = 0;
  redirectBackLink: string = '/';
  faEdit = faEdit;
  faTrash = faTrash;

  constructor(
    private appService: AppService,
    private userService: UserService,
    private purseService: PursesService,
    private route: Router
  ) {}

  ngOnInit(): void {
    this.userId = this.appService.getId();

    this.userService.getPurses(this.userId).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 200)
        {
          this.refactorDateAndPushToArray(response.body.purses)
        }
      },
      error: (err) => {
        if (err.status === 401)
        {
          this.isLoading = false;
          this.appService.NewAccessToken();
          return;
        }

        console.log("Houve um erro ao tentar buscar as carteiras " + err.message);
        this.isLoading = false;
        return;
      }
    })
  }

  refactorDateAndPushToArray(purses: any): void 
  {
    if (purses.length > 0) 
    {
      purses.forEach((purse: Purse) => {
        const date = new Date(purse.createdAt);
        purse.createdAt = date.toLocaleDateString('pt-BR');
        this.purses.push(purse);
        this.pursesQty++;
      });
      this.isLoading = false;
    }
    else {
      this.isLoading = false;
    }

    console.log(purses);
  }
  
  createPurse(): void 
  {
    this.route.navigate(["/create-purse"]);
  }
  
  redirectToViewActives(purseId: number): void 
  {
    this.route.navigate(["/view-actives/" + purseId]);
  }
  
  redirectToUpdatePurse(purseId: number): void
  {
    this.route.navigate(["/edit-purse/" + purseId]);
  }

  deletePurse(id: number): void 
  {
    this.purseService.delete(id).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 204)
        {
          this.purses = this.purses.filter(purse => purse.purse_Id !== id);
        }
        else 
        {
          console.log("Uma resposta inesperada foi retornada pelo servidor!");        
        }
      },
      error: (error) => {
        if (error.status === 404)
        {
          alert("Carteira não encontrada!")
        }
        if (error.status === 500)
        {
          alert("Ocorreu um erro ao tentar deletar a carteira!");
        }
      }
    })
  }

}
