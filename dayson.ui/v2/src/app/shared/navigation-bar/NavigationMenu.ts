export interface INavigationMenu {
    title: string;

    selected: string;
    groups: INavigationMenuGroup[];
}

export interface INavigationMenuGroup {
    title: string;
    cssClass?: string;

    items: INavigationMenuItem[];
}

export interface INavigationMenuItem {
    id: string;
    text: string;
    icon?: string;
    cssClass?: string;
    disabled?: string;
    target?: string | Array<any>;
    callback?: () => void;
    callbackArguments?: any[];
}
