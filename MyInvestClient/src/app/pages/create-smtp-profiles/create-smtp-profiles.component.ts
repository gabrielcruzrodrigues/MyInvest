import { CreateSmtpPropertie } from '@/types/CreateSmtpPropertie';
import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SmtpPropertieService } from '@services/smtp-propertie.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-smtp-profiles',
  templateUrl: './create-smtp-profiles.component.html',
  styleUrl: './create-smtp-profiles.component.scss'
})
export class CreateSmtpProfilesComponent {
  isLoading : boolean = false;
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private route: Router,
    private toastr: ToastrService,
    private smtpPropertieService: SmtpPropertieService
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      smtpClient: ['', Validators.required],
      port: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      senderEmail: ['', [Validators.required, Validators.email]],
      passwordSenderEmail: ['', Validators.required]
    })
  }

  onSubmit(): void {
    if (!this.validateFields()) {
      return;
    }

    this.isLoading = true;

    const smtpData: CreateSmtpPropertie = {
      name: this.form.value?.name,
      smtpClient: this.form.value?.smtpClient,
      port: String(this.form.value?.port),
      senderEmail: this.form.value?.senderEmail,
      passwordSenderEmail: this.form.value?.passwordSenderEmail
    }
    
    this.smtpPropertieService.createSmtpProfile(smtpData).subscribe({
      next: (response: HttpResponse<any>) => {
        this.toastr.success("Seu perfil de SMTP foi criado com sucesso!");
        this.isLoading = false;
        this.route.navigate(['/list-smtp-properties'])
      },
      error: (err) => {
        this.toastr.error("Houve um erro ao tentar criar seu perfil SMTP!");
        console.log(err);
        return;
      }
    })
  }

  validateFields(): boolean {
    const nameErrors = this.form.get('name')?.errors;
    const smtpClientErrors = this.form.get('smtpClient')?.errors;
    const portErrors = this.form.get('port')?.errors;
    const senderEmailErrros = this.form.get('senderEmail')?.errors;
    const passwordSenderEmailErrors = this.form.get('passwordSenderEmail')?.errors;

    if (nameErrors?.['required']) {
      this.toastr.error('O nome é obrigatório!');
      return false;
    }
    
    if (smtpClientErrors?.['required']) {
      this.toastr.error('O cliente SMTP é obrigatório!');
      return false;
    }
    
    if (portErrors?.['required']) {
      this.toastr.error('A porta é obrigatória!');
      return false;
    }
    
    if (portErrors?.['pattern']) {
      this.toastr.error('A porta deve ser um numero!')
      return false;
    }
    
    if (senderEmailErrros?.['required']) {
      this.toastr.error('O senderEmail é obrigatório!');
      return false;
    }
    
    if (passwordSenderEmailErrors?.['required']) {
      this.toastr.error('O passwordSenderEmail é obrigatório!');
      return false;
    }

    return true;
  }
}
