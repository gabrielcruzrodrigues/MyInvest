import { Component, OnInit } from '@angular/core';
import { Router, Event, NavigationEnd } from '@angular/router';
import { AppService } from '@services/app.service';
import { environment } from 'environments/environment';
import { GoogleAnalyticsService } from 'ngx-google-analytics';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    constructor(
        private router: Router,
        protected $gaService: GoogleAnalyticsService,
        private appService: AppService
    ) {
        this.router.events.subscribe((event: Event) => {
            if (
                event instanceof NavigationEnd &&
                environment.NODE_ENV === 'production'
            ) {
                this.$gaService.pageView(event.url);
            }
        });
    }

    ngOnInit(): void {
        var expirationTokenDate = this.appService.getExpirationTokenDate();

        if (!expirationTokenDate) {
            this.router.navigate(["/login"]);
        }

        const expirationDate = new Date(expirationTokenDate);
        const currentDate = new Date();

        if (currentDate >= expirationDate) {
            this.appService.NewAccessToken();
        }
    }
}
