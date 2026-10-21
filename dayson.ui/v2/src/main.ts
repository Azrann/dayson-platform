import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { enableProdMode } from '@angular/core';
import { environment } from './environments/environment';
import { DaysonModule } from './app/dayson.module';

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(DaysonModule);
