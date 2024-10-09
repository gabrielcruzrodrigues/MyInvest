import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpResponse } from '@angular/common/http';
import { LoadingComponent } from '../layout/loading/loading.component';
import { PurseService } from '../../services/purse.service';
import { BackComponent } from '../layout/back/back.component';

interface Purse {
  purse_Id: number;
  name: string;
  description: string;
  createdAt: string;
}

@Component({
  selector: 'app-view-purses',
  standalone: true,
  imports: [
    CommonModule, LoadingComponent, BackComponent
  ],
  templateUrl: './view-purses.component.html',
  styleUrl: './view-purses.component.scss'
})
export class ViewPursesComponent implements OnInit{
  userId: string = '';
  purses: Purse[] = [];
  isLoading: boolean = true;
  pursesQty: number = 0;
  redirectBackLink: string = '/';

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private purseService: PurseService,
    private route: Router
  ) {}

  ngOnInit(): void {
    if (!this.authService.verifyIfUserIdLogged())
    {
      this.route.navigate(["/login"])
      this.isLoading = false;
      return;
    }

    this.userId = this.authService.getId();

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
          this.authService.redirectAfterExpiredAccessToken();
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
          console.log("Uma responsta inesperada foi retornada pelo servidor!");        
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
