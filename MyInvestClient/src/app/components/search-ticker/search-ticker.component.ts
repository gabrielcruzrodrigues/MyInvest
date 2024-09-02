import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActiveService } from '../../services/active.service';
import { HttpResponse } from '@angular/common/http';

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
    private activeService: ActiveService
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
            console.log(response);
          }
        },
        error: (err) => {
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
