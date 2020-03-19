import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Story } from './story';

@Injectable({
  providedIn: 'root'
})
export class HackerNewsService {

  constructor(private http: HttpClient) { }

  /**
   * getStories - Retrieve ALL stories from hacker news api
   */
  public getStories() : Observable<Array<Story>> {
    return this.http.get<Array<Story>>('/api/hackernews');
  }

  public getRecentStories(pageNumber: number, pageSize?: number) : Observable<Array<Story>> {
    return this.http.get<Array<Story>>(`/api/hackernews/recent/${pageNumber}${pageSize && '/' + pageSize}`);
  }
}
