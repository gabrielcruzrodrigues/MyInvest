import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActiveService } from '../../services/active.service';
import { HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search-ticker',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './search-ticker.component.html',
  styleUrl: './search-ticker.component.scss'
})
export class SearchTickerComponent {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private activeService: ActiveService,
    private route: Router
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required]
    })
  }

  onSubmit()
  {
    if (this.form.valid) 
    {
      this.activeService.search(this.form.get('name')?.value).subscribe({
        next: (response: HttpResponse<any>) => {
          if (response.status === 200)
          {
            var name = this.form.get('name')?.value;
            this.route.navigate(["/view-ticker/" + name]);
          }
        },
        error: (err) => {
          if (err.status === 404)
          {
            alert("Não foi encontrado nenhum ativo com o ticker informado.");
            return;
          }
          console.log(err);
        }
      })
    }
    else 
    {
      this.form.markAllAsTouched()
    }
  }
}
