import { Component } from '@angular/core';
import { HackerNewsService } from './hacker-news.service';
import { Story } from './story';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ClientApp';

  constructor(private hackerNews: HackerNewsService) { }

  stories: Array<Story> = [];

  ngOnInit() {
    this.hackerNews.getStories().subscribe((items: Array<Story>) => {
      this.stories = items;
    }, (error) => console.log(error));
  }
}
