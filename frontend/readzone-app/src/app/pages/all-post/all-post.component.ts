import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-all-post',
  standalone: false,
  templateUrl: './all-post.component.html',
  styleUrl: './all-post.component.css'
})
export class AllPostComponent implements OnInit {
  posts: any[] = [];
  loading = true;
  error = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');

    if (!token) {
      console.error('[DEBUG] No token found in localStorage');
      this.error = 'You must be logged in to view posts.';
      this.loading = false;
      return;
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any[]>('https://localhost:7216/api/post', { headers }).subscribe({
      next: (data) => {
        console.log('[DEBUG] Posts fetched:', data);
        this.posts = data;
        
        this.posts.forEach(post => {
          this.getLikeCount(post.postId);
          this.getCommentCount(post.postId);
        });

        this.loading = false;
      },
      error: (err) => {
        console.error('[DEBUG] Error fetching posts:', err);
        this.error = 'Could not load posts.';
        this.loading = false;
      }
    });
  }

  getLikeCount(postId: string): void {
    this.http.get<number>(`https://localhost:7216/api/like/count/${postId}`).subscribe({
      next: count => {
        const post = this.posts.find(p => p.postId === postId);
        if (post) post.likeCount = count;
      },
      error: err => {
        console.error(`Error fetching likes for post ${postId}:`, err);
      }
    });
  }

  getCommentCount(postId: string): void {
    this.http.get<number>(`https://localhost:7216/api/comment/count/${postId}`).subscribe({
      next: count => {
        const post = this.posts.find(p => p.postId === postId);
        if (post) post.commentCount = count;
      },
      error: err => {
        console.error(`Error fetching comments for post ${postId}:`, err);
      }
    });
  }

  getTagline(htmlContent: string): string {
  const tempDiv = document.createElement('div');
  tempDiv.innerHTML = htmlContent || '';
  const text = tempDiv.textContent || tempDiv.innerText || '';
  const firstSentence = text.split('. ')[0];
  return firstSentence.endsWith('.') ? firstSentence : firstSentence + '.';
}

}
