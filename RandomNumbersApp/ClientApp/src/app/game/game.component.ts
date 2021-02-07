import {Component, Inject, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {CountdownConfig, CountdownEvent} from 'ngx-countdown/interfaces';
import {AuthorizeService} from '../../api-authorization/authorize.service';
import {take} from 'rxjs/operators';

@Component({
    selector: 'app-game',
    templateUrl: './game.component.html',
    styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {
    public matchResults: MatchResult[];

    public currentMatch: ParticipantMatch;

    public countDownConfig: CountdownConfig;

    public isAuth = false;

    constructor(private http: HttpClient,
                @Inject('BASE_URL') private baseUrl: string,
                private authService: AuthorizeService) {
    }

    async playNow(): Promise<void> {
        this.currentMatch = await this.http.post<ParticipantMatch>(this.baseUrl + 'match/post', {}).toPromise();
        this.initCountdownTimer();
    }

    countDownEventHandler($event: CountdownEvent): void {
        if ($event.status === 3) {
            this.refresh().then();
        }
    }

    async refresh(): Promise<void> {
        await this.asyncOnInit();
    }

    ngOnInit(): void {
        this.asyncOnInit().then();
    }

    async asyncOnInit(): Promise<void> {
        this.isAuth = await this.authService.isAuthenticated().pipe(take(1)).toPromise();
        this.matchResults = await this.getMatchResults();

        if (this.isAuth) {
            this.currentMatch = await this.getParticipantMatch();
            this.initCountdownTimer();
        }
    }

    private initCountdownTimer(): void {
        if (this.currentMatch) {
            const expirationDate = new Date(this.currentMatch.expiration);
            const diffInSec = Math.ceil((expirationDate.getTime() - new Date().getTime()) / 1000);
            this.countDownConfig = {
                leftTime: diffInSec + 1
            };
        } else {
            this.countDownConfig = null;
        }
    }

    async getMatchResults(): Promise<MatchResult[]> {
        return this.http.get<MatchResult[]>(this.baseUrl + 'match/get').toPromise();
    }

    async getParticipantMatch(): Promise<ParticipantMatch> {
        return this.http.get<ParticipantMatch>(this.baseUrl + 'match/getCurrentMatch').toPromise();
    }

    printDate(date: string): string {
        return new Date(date).toLocaleString();
    }
}

interface MatchResult {
    expiration: string;
    winnerName: string;
    winnerNumber: number;
}

interface ParticipantMatch {
    number: number;
    expiration: string;
}
