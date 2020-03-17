import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HackerNewsService {

  constructor(private http: HttpClient) { }

  /**
   * getStories - Retrieve ALL stories from hacker news api
   */
  public getStories() : Observable<object> {
    return this.http.get('/api/hackernews');
  }
}
