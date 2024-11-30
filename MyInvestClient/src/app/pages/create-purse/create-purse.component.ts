import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AppService } from '@services/app.service';
import { PursesService } from '@services/purses.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-purse',
  templateUrl: './create-purse.component.html',
  styleUrl: './create-purse.component.scss'
})
export class CreatePurseComponent {
  isLoading: boolean = false;
  form: FormGroup;
  userId: string = '';

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder,
    private appService: AppService,
    private purseService: PursesService,
    private route: Router
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      user_Id: ['']
    })
  }

  ngOnInit(): void {
    this.userId = this.appService.getId();
    this.form.patchValue({
      user_Id: this.userId
    });
  }

  onSubmit(): void {
    
    if (this.form.invalid) {

      const nameErrors = this.form.get('name')?.errors;
      const descriptionErrors = this.form.get('description')?.errors;

      if (nameErrors?.['required']) {
        this.toastr.error('O campo nome é obrigatório.');
      }
      if (nameErrors?.['minlength']) {
        this.toastr.error('O nome deve ter no mínimo 3 caracteres.');
      }

      if (descriptionErrors?.['required']) {
        this.toastr.error('O campo descrição é obrigatório.');
      }

      return;
    }

    this.isLoading = true;
    this.purseService.create(this.form.value).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 201) {
          this.isLoading = false;
          if (typeof window !== 'undefined') {
            this.toastr.success("Carteira criada com sucesso!");
          }

          this.route.navigate(["/purses"]);
        }
        else {
          this.isLoading = false;
          console.log("Houve uma resposta inesperada do servidor.");
          if (typeof window !== 'undefined') {
            this.toastr.error("Houve um problema ao criar a carteira!");
          }
        }
      },
      error: (err) => {
        if (err.status === 401) {
          this.appService.clearLocalStorage();
          return;
        }

        this.isLoading = false;
        if (typeof window !== 'undefined') {
          this.toastr.error("Houve um erro ao tentar criar uma carteira!");
        }
      }
    })
  }

}
