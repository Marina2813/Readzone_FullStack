import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private http: HttpClient) {}
  getPostById(postId: string): Observable<any> {
  return this.http.get<any>(`https://localhost:7216/api/Post/${postId}`);
}

  getPostsByCurrentUser(): Observable<any[]> {
    const token = localStorage.getItem('token');
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : undefined;
    return this.http.get<any[]>('https://localhost:7216/api/Post/myposts', { headers }); 
}
  deletePost(postId: string): Observable<any> {
    return this.http.delete(`https://localhost:7216/api/Post/${postId}`);
  }
  updatePost(postId: string, updatedData: any): Observable<any> {
    return this.http.put(`https://localhost:7216/api/Post/${postId}`, updatedData);
  }
}
