import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
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
  selector: 'app-create-active',
  templateUrl: './create-active.component.html',
  styleUrl: './create-active.component.scss'
})
export class CreateActiveComponent implements OnInit {
  isLoading: boolean = true;
  userId: string = '';
  purses: Purse[] = [];
  selectedPurseId: string = '';

  form: FormGroup;
  activeName: string = '';
  percentValue: number | null = null;
  dYDisplayValue: string = '';

  alreadySearched: boolean = false;

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
    private userService: UserService,
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

    var param = this.activedRoute.snapshot.paramMap.get('purseId');
    if (param != null)
    {
      this.selectedPurseId = param;
    } 

    this.userService.getPurses(this.userId).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 200) {
          response.body.purses.forEach((purse: any) => {
            const newPurse: Purse = {
              id: purse.purse_Id,
              name: purse.name
            };
            this.purses.push(newPurse);
          });
          this.isLoading = false;
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.log(err);
      }
    })
  }

  searchTicker() {

    if (this.form.invalid) {

      const activeNameErrors = this.form.get('activeName')?.errors;
      const percentValueErrors = this.form.get('percentValue')?.errors;

      if (activeNameErrors?.['required']) {
        this.toastr.error('O campo ticker é obrigatório.');
      }

      if (percentValueErrors?.['required']) {
        this.toastr.error('O campo DY desejado é obrigatório.');
      }

      this.alreadySearched = false;

      return;
    }

    this.isLoading = true;
    if (this.percentValue === null) {
      this.toastr.error("O DY (Dividend Yield) não pode ser nulo!");
      this.isLoading = false;
      return;
    }

    this.activeService.search(this.form.get('activeName')?.value, this.percentValue).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 200) {
          this.populateActiveFields(response.body);
          this.alreadySearched = true;
          this.isLoading = false;
        }
        else {
          this.toastr.error("Ocorreu um erro interno no sistema!");
        }
      },
      error: (err) => {
        this.isLoading = false;
        if (typeof window !== 'undefined') {
          this.alreadySearched = false;
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
  }

  addActive(): void {
    if (this.alreadySearched === false) {
      this.toastr.error("Você precisa buscar pelo ativo primeiro!");
      return;
    }

    if (this.selectedPurseId === '') {
      this.toastr.error("Selecione uma carteira!");
      return;
    }

    if (this.form.get('activeName')?.value === '') {
      this.toastr.error("Você precisa adicionar um ticker primeiro!");
      return;
    }

    if (this.percentValue === null) {
      this.toastr.error("O DY desejado não pode ser nulo!");
      return;
    }

    this.isLoading = true;

    this.activeService.create(this.selectedPurseId, this.active.tipo, this.active.ativo, this.percentValue?.toString()).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 201) {
          this.isLoading = false;
          this.router.navigate(["/purses"]);
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.log(err);
      }
    });
  }

  onInputChange(event: any): void {
    const inputValue = event.target.value.replace('%', '').trim();
    const numericValue = parseFloat(inputValue);

    if (numericValue !== this.percentValue && numericValue > 0) {
      this.percentValue = numericValue;
      this.dYDisplayValue = `${numericValue}%`;
    }

    if (numericValue === 0 || numericValue < 0 && numericValue === null && isNaN(numericValue)) {
      this.toastr.error("DY incorreto!");
    }
    this.isLoading = false;
  }
}
