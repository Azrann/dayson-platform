import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { IWebUserDetails } from '../data/user/web-user-details';
import { WebUserService } from '../services/web-user/web-user.service';
import { NavigationBarService } from './navigration-bar.service';

@Component({
    selector: 'daysonapp-navigation-bar',
    templateUrl: './navigation-bar.component.html',
    styleUrls: ['./navigation-bar.component.scss']
})
export class NavigationBarComponent implements OnInit, OnDestroy {
    @Output() toggle: EventEmitter<any> = new EventEmitter();
    authUserSubscription: Subscription;

    user: IWebUserDetails;

    showMenu: boolean;

    constructor(
        private navigationBarService: NavigationBarService,
        private webUserService: WebUserService,
        private router: Router,
    ) { }

    ngOnInit() {
        this.authUserSubscription = this.webUserService.getCurrentUserDetails().subscribe((user: IWebUserDetails) => {
            this.user = user;
        });

        this.navigationBarService.navMenuUpdated.subscribe((menu) => {
            this.showMenu = !!menu.selected;
        });
    }

    ngOnDestroy() {
        this.toggle.complete();
        this.authUserSubscription.unsubscribe();
    }

    menuClicked($event: Event) {
        this.navigationBarService.reportNavbarMenuClicked();
        this.toggle.emit();
    }

    goToUser() {
        if (this.showMenu) {
            this.router.navigate(['admin', 'profile']);
        }
    }
}
