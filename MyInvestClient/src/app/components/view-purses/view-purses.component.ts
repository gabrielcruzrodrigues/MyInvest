import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../layout/navbar/navbar.component';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpResponse } from '@angular/common/http';

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
    NavbarComponent, CommonModule
  ],
  templateUrl: './view-purses.component.html',
  styleUrl: './view-purses.component.scss'
})
export class ViewPursesComponent implements OnInit{
  purse: Purse = {
    purse_Id: 1,
    name: "Luxury Leather Bag",
    description: "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
    createdAt: "2024-08-31T10:00:00Z",
  }
  userId: string = '';
  purses: Purse[] = [];


  constructor(
    private authService: AuthService,
    private userService: UserService,
    private route: Router
  ) {}

  ngOnInit(): void {
    // this.purses.push(this.purse, this.purse, this.purse, this.purse);
    // if (!this.authService.verifyIfUserIdLogged())
    // {
    //   this.route.navigate(["/create-account"])
    // }
    this.userId = this.authService.getId();
    this.userService.getPurses(this.userId).subscribe({
      next: (response: HttpResponse<any>) => {
        console.log(response);
        if (response.status === 200)
        {
          this.refactorDateAndPushToArray(response.body.purses)
        }
      },
      error: (err) => {
        alert("Houve um erro ao tentar buscar as carteiras do usuário.");
        console.log(err);
      }
    })
  }

  refactorDateAndPushToArray(purses: any): void 
  {
    purses.forEach((purse: Purse) => {
      const date = new Date(purse.createdAt);
      purse.createdAt = date.toLocaleDateString('pt-BR');
      this.purses.push(purse);
    });
  }

  redirectToPurse()
  {

  }




}
