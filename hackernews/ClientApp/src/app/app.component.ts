import { Component } from '@angular/core';
import { HackerNewsService } from './hacker-news.service';
import { Story } from './story';
import { HackerNewsDataSource } from './hacker-news-data-source';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(private hackerNews: HackerNewsService) { }

  stories = new HackerNewsDataSource(this.hackerNews);
}
