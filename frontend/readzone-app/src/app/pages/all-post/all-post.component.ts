import { Component, OnInit} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-all-post',
  standalone: false,
  templateUrl: './all-post.component.html',
  styleUrl: './all-post.component.css'
})
export class AllPostComponent  implements OnInit{
  posts: any[] = [];
  loading = true;
  error = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any[]>('https://localhost:7216/api/post', { headers }).subscribe({
      next: (data) => {
        console.log('[DEBUG] Posts fetched:', data);
        this.posts = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('[DEBUG] Error fetching posts:', err);
        this.error = 'Could not load posts.';
        this.loading = false;
      }
    });
  } 
}


