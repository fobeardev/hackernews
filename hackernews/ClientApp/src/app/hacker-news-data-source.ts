import { DataSource, CollectionViewer } from '@angular/cdk/collections';
import { Observable, BehaviorSubject, Subscription } from 'rxjs';
import { Story } from './story';
import { HackerNewsService } from './hacker-news.service';

export class HackerNewsDataSource extends DataSource<Story | undefined> {
  private _length = 500;
  private _pageSize = 30;
  private _cachedData = Array.from<Story>({length: this._length});
  private _fetchedPages = new Set<number>();
  private _dataStream = new BehaviorSubject<(Story | undefined)[]>(this._cachedData);
  private _subscription = new Subscription();

  constructor(private hackerNews: HackerNewsService) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<(Story | undefined)[]> {
    this._subscription.add(collectionViewer.viewChange.subscribe(range => {
      const startPage = this._getPageForIndex(range.start);
      const endPage = this._getPageForIndex(range.end - 1);
      for (let i = startPage; i <= endPage; i++) {
        this._fetchPage(i);
      }
    }));
    return this._dataStream;
  }

  disconnect(): void {
    this._subscription.unsubscribe();
  }

  private _getPageForIndex(index: number): number {
    return Math.floor(index / this._pageSize);
  }

  private _fetchPage(page: number) {
    if (this._fetchedPages.has(page)) {
      return;
    }

    this._fetchedPages.add(page);

    this.hackerNews.getRecentStories(page, this._pageSize).subscribe(stories => {
      this._cachedData.splice(page * this._pageSize, this._pageSize, ...stories);

      this._dataStream.next(this._cachedData);
    });

  }
}
