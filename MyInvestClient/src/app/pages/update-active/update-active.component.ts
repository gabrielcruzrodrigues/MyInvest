import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ActiveService } from '@services/active.service';
import { AppService } from '@services/app.service';
import { UserService } from '@services/user.service';
import { ToastrService } from 'ngx-toastr';

interface Active {
  data: string,
  ativo: string,
  nomeDoAtivo: string,
  tipo: string,
  dividentYield: string,
  precoAtual: string,
  p_VP: string,
  preco_Teto: string,
  indicacao: string,
  p_L: string,
  roe: string,
  crecimento_De_Dividendos_5_anos: string,
  proventos_pagos: string
}

interface Purse {
  id: string,
  name: string
}

@Component({
  selector: 'app-update-active',
  templateUrl: './update-active.component.html',
  styleUrl: './update-active.component.scss'
})
export class UpdateActiveComponent {
  userId: string = '';
  activeName: string = '';
  activeId: string = '';
  form: FormGroup;

  isLoading: boolean = true;

  percentValue: number | null = null;
  dYDisplayValue: string = '';

  hasUpdatedInputAutomatically: boolean = false;

  active: Active = {
    data: '',
    ativo: '',
    nomeDoAtivo: '',
    tipo: '',
    dividentYield: '',
    precoAtual: '',
    p_VP: '',
    preco_Teto: '',
    indicacao: '',
    p_L: '',
    roe: '',
    crecimento_De_Dividendos_5_anos: '',
    proventos_pagos: ''
  }

  constructor(
    private activeService: ActiveService,
    private appService: AppService,
    private activedRoute: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private toastr: ToastrService,
  ) {
    this.form = this.fb.group({
      activeName: ['', Validators.required],
      percentValue: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.userId = this.appService.getId();
    var activeNameParam = this.activedRoute.snapshot.paramMap.get('name');
    var percentValueParam = this.activedRoute.snapshot.paramMap.get('percentValue');
    var activeIdParam = this.activedRoute.snapshot.paramMap.get('activeId');

    activeNameParam !== null ? this.activeName = activeNameParam : this.toastr.error("Aconteceu um erro ao tentar buscar o ticker!");
    percentValueParam !== null ? this.percentValue = parseInt(percentValueParam) : this.toastr.error("Aconteceu um erro ao tentar buscar o ticker!");
    activeIdParam !== null ? this.activeId = activeIdParam : this.toastr.error("Aconteceu um erro ao tentar buscar o ticker!");

    this.form.patchValue({ percentValue: this.percentValue, activeName: this.activeName });

    this.searchTicker();
  }

  searchTicker() {
    const param = this.form.get('percentValue')?.value;

    if (param === 0 || param < 0 || param === null || isNaN(param)) {
      this.toastr.error("DY invalido!");
      return;
    }

    this.isLoading = true;
    this.activeService.search(this.activeName, this.percentValue).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 200) {
          this.populateActiveFields(response.body);
          this.isLoading = false;
        }
        else {
          this.toastr.error("Ocorreu um erro interno no sistema!");
        }
      },
      error: (err) => {
        this.isLoading = false;
        if (typeof window !== 'undefined') {
          this.toastr.error("Aconteceu um erro ao tentar buscar o ticker!");
        }
      }
    })
  }

  populateActiveFields(body: any): void {
    this.active = {
      data: body.data || '',
      ativo: body.ativo || '',
      nomeDoAtivo: body.nomeDoAtivo || '',
      tipo: body.tipo || '',
      dividentYield: body.dividentYield || '',
      precoAtual: body.precoAtual || '',
      p_VP: body.p_VP || '',
      preco_Teto: body.preco_Teto || '',
      indicacao: body.indicacao || '',
      p_L: body.p_L || '',
      roe: body.roe || body.roe,
      crecimento_De_Dividendos_5_anos: body.crecimento_De_Dividendos_5_anos || '',
      proventos_pagos: body.proventos_pagos || ''
    }
    if (!this.hasUpdatedInputAutomatically) {
      this.dYDisplayValue = body.dividentYield;
      this.hasUpdatedInputAutomatically = false;
    }

    this.isLoading = false;
  }

  updateActive(): void {
    this.isLoading = true;

    if (this.form.get('percentValue')?.value === '') {
      this.toastr.error("O DY não pode ser nulo");
      this.isLoading = false;
      return;
    }

    this.activeService.update(this.activeId, this.percentValue).subscribe({
      next: (response: HttpResponse<any>) => {
        this.isLoading = false;
        if (response.status === 204) {
          this.router.navigate(["/purses"]);
        }
      },
      error: (err) => {
        this.isLoading = false;
      }
    })
  }

  onInputChange(event: any): void {
    const inputValue = event.target.value.replace('%', '').trim();
    const numericValue = parseFloat(inputValue);

    if (numericValue !== this.percentValue && numericValue > 0) {
      this.percentValue = numericValue;
      this.dYDisplayValue = `${numericValue}%`;
    }

    if (numericValue === null || isNaN(numericValue)) {
      this.toastr.error("O DY deve ser numérico e não pode estar em branco!");
      return;
    }
    
    if (numericValue === 0 || numericValue < 0) {
      this.toastr.error("DY invalido!");
      return;
    }
  }
}