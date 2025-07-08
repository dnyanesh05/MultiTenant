import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
