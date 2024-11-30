import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { sleep } from '@/utils/helpers';

import { createUserWithEmailAndPassword } from '@firebase/auth';
import {
    User,
    onAuthStateChanged,
    signInWithEmailAndPassword,
    signInWithPopup
} from 'firebase/auth';
import { GoogleAuthProvider } from 'firebase/auth';
import { firebaseAuth } from '@/firebase';
import { environment } from 'environments/environment.prod';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

const provider = new GoogleAuthProvider();

@Injectable({
    providedIn: 'root'
})
export class AppService {
    public user?: any | null = null;
    url: string = environment.URL + '/Auth/';

    constructor(
        private router: Router,
        private toastr: ToastrService,
        private http: HttpClient
    ) {
        onAuthStateChanged(
            firebaseAuth,
            (user) => {
                if (user) {
                    this.user = user;
                } else {
                    this.user = undefined;
                }
            },
            (e) => {
                this.user = undefined;
            }
        );
    }

    async registerWithEmail(username: string, email: string, password: string) {
        const urlForRequest = this.url + "register";
        var data = { username: username, email: email, password: password }


        this.http.post(urlForRequest, data).subscribe({
            next: (response: HttpResponse<any>) => {
                this.configureLocalStorage(response);
                this.user = response.body;
                this.router.navigate(['/']);

            },
            error: (error: HttpErrorResponse) => {
                if (error.status === 401) {
                    this.toastr.error("Credenciais incorretas!");
                }
                if (error.status === 400) {
                    this.toastr.error(error.error.message);
                }
            }
        });
    }

    loginWithEmail(email: string, password: string) {
        const urlForRequest = this.url + "login";
        var data = { email: email, password: password }


        this.http.post(urlForRequest, data).subscribe({
            next: (response: HttpResponse<any>) => {
                this.configureLocalStorage(response);
                this.user = response.body;
                this.router.navigate(['/']);

            },
            error: (error) => {
                if (error.status === 401) {
                    this.toastr.error("Credenciais incorretas!");
                }
                console.log(error.message);
            }
        });
    }

    async signInByGoogle() {
        try {
            const result = await signInWithPopup(firebaseAuth, provider);
            this.user = result.user;
            this.router.navigate(['/']);

            return result;
        } catch (error) {
            this.toastr.error(error.message);
        }
    }

    async getProfile() {
        // try {
        //     await sleep(500);
        //     if (!this.user) {
        //         this.logout();
        //     }
        // } catch (error) {
        //     this.logout();
        //     this.toastr.error(error.message);
        // }
        return true;
    }

    // async logout() {
    //     await firebaseAuth.signOut();
    //     this.user = null;
    //     this.router.navigate(['/login']);
    // }

    configureLocalStorage(body: any): void {
        if (body) {
            localStorage.setItem('userId', body.userId);
            localStorage.setItem('Token', body.token);
            localStorage.setItem('RefreshToken', body.refreshToken);
            localStorage.setItem('Expiration', body.expiration);
        }
    }

    verifyIfUserIdLogged(): boolean {
        if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
            var token = localStorage.getItem('Token');
            var userId = localStorage.getItem('userId');
            if (token != null && userId != null) {
                return true;
            };
        }
        return false;
    }

    getId() {
        if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
            const userId = localStorage.getItem('userId');
            return userId !== null ? userId : '';
        }
        return '';
    }

    getExpirationTokenDate() {
        if (!this.verifyIfUserIdLogged()) {
            return null;
        }

        if (typeof window == 'undefined' || typeof window.localStorage == 'undefined') {
            return null;
        }

        return localStorage.getItem('Expiration');

    }

    NewAccessToken() {
        var expiredAccessToken = localStorage.getItem('Token');
        var refreshToken = localStorage.getItem('RefreshToken');

        if (!refreshToken) {
            this.clearLocalStorage();
            return;
        }

        var urlForRequest = this.url + "new-access-token";

        var objectForRequest = {
            accessToken: expiredAccessToken,
            refreshToken: refreshToken
        }

        this.http.post(urlForRequest, objectForRequest, { observe: 'response' }).subscribe({
            next: (response: HttpResponse<any>) => {
                localStorage.removeItem('RefreshToken');
                localStorage.removeItem('Expiration');
                localStorage.removeItem('Token');

                localStorage.setItem('Token', response.body.accessToken);
            },
            error: (error: any) => {
                if (error.status === 400) {
                    this.clearLocalStorage();
                }
                console.log(`houve um erro ao tentar se comunicar com o servidor! err: ${error.message}`);
            }
        });
    }

    clearLocalStorage(): void {
        localStorage.removeItem('RefreshToken');
        localStorage.removeItem('Expiration');
        localStorage.removeItem('userId');
        localStorage.removeItem('Token');
    }

    createAccount(data: any): Observable<any> {
        const urlForRequest = this.url + "register";
        return this.http.post(urlForRequest, data, { observe: 'response' });
    }

    // login(data: any): Observable<any> {
    //     const urlForRequest = this.url + "login";
    //     return this.http.post(urlForRequest, data, { observe: 'response' });
    // }

    logout(): void {
        if (typeof window == 'undefined' || typeof window.localStorage == 'undefined') {
            return;
        }

        localStorage.clear();
        this.router.navigate(["/login"]);
    }

    getHeaders() {
        if (typeof window == 'undefined' || typeof window.localStorage == 'undefined') {
            return;
        }

        const token = localStorage.getItem('Token');

        if (token) {
            return new HttpHeaders().set('Authorization', `Bearer ${token}`);
        }
        return new HttpHeaders();
    }

    requestCodeToLogin(email: string) : Observable<any> {
        const urlForRequest = this.url + "request-login-by-code";
        return this.http.post(urlForRequest, { userEmail: email }, { observe: 'response'});
    }
}
