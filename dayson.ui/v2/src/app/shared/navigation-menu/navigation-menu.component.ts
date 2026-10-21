import { Subject } from 'rxjs/Subject';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/operator/takeUntil';

import { INavigationMenu, INavigationMenuItem } from '../navigation-bar/NavigationMenu';
import { NavigationBarService } from './../navigation-bar/navigration-bar.service';
import { Component, OnDestroy, OnInit, Output, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';

@Component({
    selector: 'daysonapp-navigation-menu',
    templateUrl: './navigation-menu.component.html',
    styleUrls: ['./navigation-menu.component.scss']
})
export class NavigationMenuComponent implements OnInit, OnDestroy {
    @Output() toggle: EventEmitter<any> = new EventEmitter();
    private subscription: Subscription;

    navigationMenu: INavigationMenu;

    constructor(
        private navigationBarService: NavigationBarService,
        private router: Router,
    ) { }

    ngOnInit() {
        this.navigationMenu = null;

        this.subscription = this.navigationBarService.navMenuUpdated
            .subscribe((navigationMenu) => {
                // console.log('navigationmenu received', navigationMenu);

                this.navigationMenu = navigationMenu;
            });
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.toggle.complete();
    }

    itemClicked(item: INavigationMenuItem) {
        this.toggle.emit();
        Observable.timer(300).subscribe(() => {
            if (item.callback) {

                item.callback.call(this, item.callbackArguments);
            }
            if (item.target) {
                this.router.navigate(<any[]>item.target);
            }
        });
    }
}
