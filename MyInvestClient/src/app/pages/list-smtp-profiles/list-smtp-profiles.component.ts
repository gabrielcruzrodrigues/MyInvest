import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { ActiveService } from '@services/active.service';
import { SmtpPropertieService } from '@services/smtp-propertie.service';
import { ToastrService } from 'ngx-toastr';

interface SmtpProperties {
  name: string,
  smtpClient: string,
  port: string,
  senderEmail: string,
  passwordSenderEmail: string
}

@Component({
  selector: 'app-list-smtp-profiles',
  templateUrl: './list-smtp-profiles.component.html',
  styleUrl: './list-smtp-profiles.component.scss'
})
export class ListSmtpProfilesComponent implements OnInit {
  isLoading: boolean = true;
  smtpProperties: SmtpProperties[] = [];
  faEdit = faEdit;
  faTrash = faTrash;

  constructor(
    private activedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private SmtpService: SmtpPropertieService
  ) { }

  ngOnInit(): void {
    this.SmtpService.getAllSmtpProperties().subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 200) {
          this.populateTheArrayOfActives(response.body);
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.toastr.error("Houve um erro ao tentar buscar os perfis Smtp");
      }
    })
  }

  populateTheArrayOfActives(body: any): void {
    if (body.length > 0) {
      this.smtpProperties = body.map((smtp: any) => {
        return {
          name: smtp.name,
          smtpClient: smtp.smtpClient,
          port: smtp.port,
          senderEmail: smtp.senderEmail,
          passwordSenderEmail: smtp.passwordSenderEmail
        }
      });
      this.isLoading = false;
    }
    else {
      this.isLoading = false;
    }
  }

  updateSmtpPropertie(): void {

  }

  deleteSmtpPropertie(): void {

  }
}
