import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-post-detail',
  standalone: false,
  templateUrl: './post-detail.component.html',
  styleUrl: './post-detail.component.css'
})

export class PostDetailComponent implements OnInit{
  post: any = null;
  loading: boolean = true;
  error: string = '';

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    const postId = this.route.snapshot.paramMap.get('id');
    console.log('Fetching post with ID:', postId);

    if (postId) {

      const token = localStorage.getItem('token'); 
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

      this.http.get<any>(`https://localhost:7216/api/post/${postId}`, { headers }).subscribe({
        next: (res) => {
          console.log('Post fetched:', res);
          this.post = res;
          this.loading = false;
        },
        error: (err) => {
          console.error('Error fetching post:', err);
          this.error = 'Post not found or server error.';
          this.loading = false;
        }
      });
    }else {
      this.error = 'No post ID provided.';
      this.loading = false;
  }
}
}