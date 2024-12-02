import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Route, Router } from '@angular/router';
import { AppService } from '@services/app.service';
import { HttpResponse } from '@angular/common/http';
import { SpinnerComponent } from "../../components/spinner/spinner.component";

@Component({
  selector: 'app-request-recover-password',
  templateUrl: './request-recover-password.component.html',
  styleUrl: './request-recover-password.component.scss',
})
export class RequestRecoverPasswordComponent implements OnInit {
  isLoading: boolean = true;

  constructor(
    private toast: ToastrService, 
    private route: Router,
    private appService: AppService
  ) {}

  ngOnInit(): void {
    const userId = this.appService.getId();
    this.appService.requestRecoverPassword(userId).subscribe({
      next: (response: HttpResponse<any>) => {
        this.isLoading = false;
        this.toast.success("Um link de confirmação foi enviado para o seu email cadastrado!");
        this.route.navigate(['/']);
      },
      error: (err) => {
        console.log(err);
        this.isLoading = false;
        this.toast.error("Aconteceu um erro no processo de recuperação, tente mais tarde!");
        this.route.navigate(['/']);
      }
    })

  }
}
