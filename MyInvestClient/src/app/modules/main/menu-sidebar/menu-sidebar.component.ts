import {AppState} from '@/store/state';
import {UiState} from '@/store/ui/state';
import {Component, HostBinding, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {AppService} from '@services/app.service';
import {User} from 'firebase/auth';
import {Observable} from 'rxjs';

const BASE_CLASSES = 'main-sidebar elevation-4';
@Component({
    selector: 'app-menu-sidebar',
    templateUrl: './menu-sidebar.component.html',
    styleUrls: ['./menu-sidebar.component.scss']
})
export class MenuSidebarComponent implements OnInit {
    @HostBinding('class') classes: string = BASE_CLASSES;
    public ui: Observable<UiState>;
    public user?: User;
    public menu = MENU;

    constructor(
        public appService: AppService,
        private store: Store<AppState>
    ) {}

    ngOnInit() {
        this.ui = this.store.select('ui');
        this.ui.subscribe((state: UiState) => {
            this.classes = `${BASE_CLASSES} ${state.sidebarSkin}`;
        });
        this.user = this.appService.user;
    }
}

export const MENU = [
    {
        name: 'Dashboard',
        iconClasses: 'fas fa-tachometer-alt',
        path: ['/']
    },
    {
        name: 'Buscar ativos ',
        iconClasses: 'fas fa-search-dollar',
        path: ['/create-active']
    },
    {
        name: 'Carteiras ',
        iconClasses: 'fas fa-bookmark',
        path: ['/purses']
    },
    {
        name: 'Gerenciamento da conta',
        iconClasses: 'fas fa-user',
        children: [
            {
                name: 'Alterar senha',
                iconClasses: 'fas fa-key',
                path: ['/request-recover-password']
            },
        ]
    },
    {
        name: 'Painel administrativo',
        iconClasses: 'fa-solid fa-lock',
        children: [
            {
                name: 'Gerenciamento SMTP',
                iconClasses: 'fas fa-key',
                path: ['/smtp-panel']
            }
        ]
    }
];
