import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@services/app.service';
import { PursesService } from '@services/purses.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-purse',
  templateUrl: './edit-purse.component.html',
  styleUrl: './edit-purse.component.scss'
})
export class EditPurseComponent {
  isLoading: boolean = true;
  form: FormGroup;
  userId: string = '';
  purseId: string = '';

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder,
    private purseService: PursesService,
    private appService: AppService,
    private route: Router,
    private activatedRoute: ActivatedRoute
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      user_Id: ['']
    })
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoading = true;
      this.purseService.update(this.form.value).subscribe({
        next: (response: HttpResponse<any>) => {
          if (response.status === 204) {
            this.isLoading = false;
            this.toastr.success("Carteira atualizada com sucesso!");
            this.route.navigate(["/purses"]).then(() => {
              location.reload();
            });
            return;
          }
          else {
            this.isLoading = false;
            this.toastr.error("Foi retornada uma resposta inesperada pelo servidor!");
          }
        },
        error: (err) => {
          this.isLoading = false;
          this.toastr.error("Aconteceu um erro ao atualizar a carteira! \n" + err.message);
        }
      })
    }
    else {
      this.form.markAllAsTouched();
    }
  }

  ngOnInit(): void {
    var param = this.activatedRoute.snapshot.paramMap.get('id');
    if (param !== null) {
      this.purseId = param;
    }

    this.userId = this.appService.getId();

    this.purseService.getById(this.purseId).subscribe({
      next: (response: HttpResponse<any>) => {
        this.populateForm(response.body);
      },
      error: (err) => {
        if (err.status === 404) {
          this.toastr.error("Carteira não encontrada!");
        }
        if (err.status === 500) {
          this.toastr.error("Houve um erro ao buscar as carteiras!");
        }
        this.isLoading = false;
      }
    });
  }

  populateForm(body: any) {
    this.form.patchValue({
      name: body.name,
      description: body.description,
      user_Id: body.purse_Id
    })
    this.isLoading = false;
  }
}
