import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {GameComponent} from './game/game.component';
import {ApiAuthorizationModule} from 'src/api-authorization/api-authorization.module';
import {AuthorizeInterceptor} from 'src/api-authorization/authorize.interceptor';
import {CountdownGlobalConfig, CountdownModule} from 'ngx-countdown';


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        GameComponent
    ],
    imports: [
        BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
        HttpClientModule,
        FormsModule,
        ApiAuthorizationModule,
        RouterModule.forRoot([
            {path: '', component: GameComponent, pathMatch: 'full'},
        ]),

        CountdownModule
    ],
    providers: [
        {provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true},
        {provide: CountdownGlobalConfig}
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
