import { PubNubAngular } from 'pubnub-angular2';
import { NgForm } from '@angular/forms/src/directives';

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/timer';
import { Observable } from 'rxjs/Observable';

import { Subscription } from 'rxjs/Subscription';
import { AuthUserService } from '../../shared/services/auth-user/auth-user.service';
import { ISignInResponse } from '../../shared/services/auth-user/dtos/SignInDtos';
import { Response } from '@angular/http';

@Component({
    selector: 'daysonapp-login-sign-in',
    templateUrl: './sign-in.component.html',
    styleUrls: ['./sign-in.component.scss'],
    // providers: [PubNubAngular],
})
export class SignInComponent implements OnInit {
    loginError: string;
    lastMessage: string;
    loading: boolean;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private loginService: AuthUserService,
        private pubnub: PubNubAngular,
    ) {
    }

    ngOnInit(): void {
        // this.pubnub.init({
        //     publishKey: 'REPLACE_WITH_YOUR_VALUE',
        //     subscribeKey: 'REPLACE_WITH_YOUR_VALUE',
        // });

        // this.pubnub.subscribe({
        //     channels: ['test-channel'],
        //     triggerEvents: ['message'],
        // });

        // Observable.timer(0, 1000).subscribe((c) => {
        //     this.pubnub.publish({
        //         channel: 'test-channel',
        //         message: { content: `Message #${c}` },
        //     }, (status, response) => {
        //         if (status.error) {
        //             console.log(status);
        //         } else {
        //             console.log('Message sent', response);
        //         }
        //     });
        // });

        // setTimeout(() => {
        //     this.pubnub.getMessage('test-channel', (message) => {
        //         this.lastMessage = message.message.content;
        //     });
        // }, 3000);
    }

    login(form: NgForm) {
        if (form.invalid || this.loading) {
            return;
        }

        this.loginError = null;
        this.loading = true;
        this.loginService.login(
            form.value.username,
            form.value.password,
            form.value.rememberMe
        ).subscribe((result: ISignInResponse) => {
            this.loading = false;

            if (result.error) {
                this.loginError = `${result.error} - ${result.error_description}`;
                return;
            }

            this.router.navigate(['/home']);
        }, (error: Response) => {
            this.loading = false;

            console.log(error);

            if (error.status === 401) {
                this.loginError = 'Invalid user/password';
            } else {
                this.loginError = 'Error processing request';
            }

        });
    }
}
