import { EventEmitter, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { IWebUserDetails } from '../data/user/web-user-details';
import { WebUserService } from '../services/web-user/web-user.service';
import { INavigationMenu, INavigationMenuGroup, INavigationMenuItem } from './NavigationMenu';

@Injectable()
export class NavigationBarService {
    // Observable sources
    private navbarMenuClickedSource: EventEmitter<any> = new EventEmitter<any>();
    private navMenuUpdatedSource: EventEmitter<INavigationMenu> = new EventEmitter<INavigationMenu>();

    // Observable streams
    private navbarMenuClickedObservable: Observable<any> = this.navbarMenuClickedSource.asObservable();
    private navMenuUpdatedObservable: Observable<INavigationMenu> = this.navMenuUpdatedSource.asObservable();

    public authUser: IWebUserDetails;

    // menu
    private menu: INavigationMenu;

    public constructor(
        private router: Router,
        private webUserService: WebUserService,
    ) {
    }

    get navbarMenuClicked(): Observable<any> {
        return this.navbarMenuClickedObservable;
    }

    // Service message commands
    reportNavbarMenuClicked() {
        this.navbarMenuClickedSource.emit();
    }

    get navMenuUpdated(): Observable<INavigationMenu> {
        return this.navMenuUpdatedObservable;
    }

    // set menu items
    setNavMenu(menu: INavigationMenu, selected: string) {
        if (this.authUser) {
            this.setDefaultMenu(selected);
        } else {
            this.webUserService.getCurrentUserDetails().subscribe((user: IWebUserDetails) => {
                this.authUser = user;
                this.setDefaultMenu(selected);
            });
        }
    }

    private setDefaultMenu(selected: string) {
        this.menu = <INavigationMenu>{
            selected,
            title: 'Team administration',
            groups: [
                <INavigationMenuGroup>{
                    title: 'ME',
                    items: [
                        // <INavigationMenuItem>{ text: 'Home', target: ['home'] },
                        <INavigationMenuItem>{
                            id: 'holidays-view',
                            text: 'My holidays',
                            target: ['holidays', 'view', 'user', this.authUser.id, 'year', '2017']
                        },
                        <INavigationMenuItem>{
                            id: 'admin-user-details-view-profile',
                            text: 'My account',
                            target: ['admin', 'profile']
                        },
                    ]
                },
                <INavigationMenuGroup>{
                    title: 'MY TEAM',
                    items: [
                        <INavigationMenuItem>{
                            id: 'holidays-team-view',
                            text: 'Team Holidays',
                            target: ['holidays/view/team']
                        },
                        <INavigationMenuItem>{
                            id: 'holidays-approve-view',
                            text: 'Holiday request manager',
                            target: ['holidays', 'approve']
                        },
                    ]
                },
                <INavigationMenuGroup>{
                    title: 'SYSTEM ADMIN',
                    items: [
                        <INavigationMenuItem>{ id: 'admin-team-view', text: 'Teams administration', target: ['admin/teams'] },
                        // <INavigationMenuItem>{ id: 'admin-company-view', text: 'Employee allowances', target: ['admin', 'company'] },
                        <INavigationMenuItem>{ id: 'admin-company-view', text: 'Company administration', target: ['admin', 'company'] },
                    ]
                },
                <INavigationMenuGroup>{
                    items: [
                        <INavigationMenuItem>{ text: 'Logout', cssClass: 'logout', callback: () => { this.logOut(); } },
                    ]
                },
            ]
        };

        this.navMenuUpdatedSource.emit(this.menu);
    }

    private logOut() {
        this.router.navigate(['login/signin']);
    }
}

