import { HttpResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ActiveService } from '../../services/active.service';

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
  crecimentoDeDividendos: string
}

@Component({
  selector: 'app-view-ticker-final',
  standalone: true,
  imports: [],
  templateUrl: './view-ticker-final.component.html',
  styleUrl: './view-ticker-final.component.scss'
})
export class ViewTickerFinalComponent implements OnInit{
  activeName: string = '';

  constructor(
    private activedRoute: ActivatedRoute,
    private activeService: ActiveService
  ) {}

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
    crecimentoDeDividendos: ''
  }

  ngOnInit(): void {
    var param = this.activedRoute.snapshot.paramMap.get('name');

    param !== null ? this.activeName = param : alert("Aconteceu um erro ao tentar buscar o ticker!");

    this.activeService.search(this.activeName).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 200)
        {
          this.populateActiveFields(response.body);
        }
        else 
        {
          alert("Ocorreu um erro interno no sistema!");
        }
      },
      error: (err) => {
        alert("Aconteceu um erro ao tentar buscar o ticker!");
      }
    })
  }

  populateActiveFields(body: any): void
  {
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
      roe: body.roe || '',
      crecimentoDeDividendos: body.crecimentoDeDividendos || ''
    } 
  }
}
