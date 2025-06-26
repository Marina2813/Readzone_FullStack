import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { CommentService, Comment } from '../../services/comment.service';

@Component({
  selector: 'app-post-detail',
  standalone: false,
  templateUrl: './post-detail.component.html',
  styleUrl: './post-detail.component.css'
})

export class PostDetailComponent implements OnInit{
  post: any = null;
  postId: string = '';
  loading: boolean = true;
  error: string = '';
  likeCount: number = 0;
  userLiked: boolean = false;

  comments: Comment[] = [];
  newComment: Partial<Comment> = {
    content: ''
  };

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private commentService: CommentService
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
          this.postId = postId;
          this.loadComments();
          this.loading = false;
          this.fetchLikeCount();
          this.checkIfUserLiked();
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
 getFormattedContent(content: string): string {
    return content?.replace(/\n/g, '<br>') || '';}

  loadComments(): void {
  if (!this.postId) return;

  this.commentService.getComments(this.postId).subscribe({
    next: (res) => {
      this.comments = res;
    },
    error: (err) => {
      console.error('Failed to load comments:', err);
    }
  });
}

addComment(): void {
    const trimmedContent = (this.newComment.content || '').trim();

    if (!trimmedContent) return;

    const payload = {
      content: trimmedContent
    };

    this.commentService.addComment(this.postId, payload).subscribe({
      next: () => {
        this.newComment.content = '';
        this.loadComments();
      },
      error: (err) => {
        console.error('Failed to add comment:', err);
      }
    });
  }



deleteComment(commentId?: number): void {
    if (commentId === undefined) return;

    if (!confirm('Are you sure you want to delete this comment?')) return;

    this.commentService.deleteComment(commentId).subscribe({
      next: () => {
        this.comments = this.comments.filter(c => c.commentId !== commentId);
      },
      error: (err) => {
        console.error('Failed to delete comment:', err);
      }
    });
  }

  fetchLikeCount(): void {
  if (!this.postId) return;

  this.http.get<number>(`https://localhost:7216/api/like/count/${this.postId}`).subscribe({
    next: (count) => {
      this.likeCount = count;
    },
    error: (err) => {
      console.error('Error fetching like count:', err);
    }
  });
}
toggleLike(): void {
  const token = localStorage.getItem('token');
  const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

  const payload = {
    postId: this.postId
  };

  this.http.post(`https://localhost:7216/api/like/toggle`, payload, { headers }).subscribe({
    next: () => {
      this.fetchLikeCount(); 
      this.userLiked = !this.userLiked; 
    },
    error: (err) => {
      console.error('Error toggling like:', err);
    }
  });
}
checkIfUserLiked(): void {
  const token = localStorage.getItem('token');
  const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

  this.http.get<boolean>(`https://localhost:7216/api/like/userliked/${this.postId}`, { headers }).subscribe({
    next: (liked) => this.userLiked = liked,
    error: (err) => console.error('Error checking user like status:', err)
  });
}



  expandTextarea(event: FocusEvent) {
  const textarea = event.target as HTMLTextAreaElement;
  textarea.rows = 4;
}

shrinkTextarea(event: FocusEvent) {
  const textarea = event.target as HTMLTextAreaElement;
  if (!textarea.value.trim()) textarea.rows = 1;
}

getTagline(htmlContent: string): string {
  const tempDiv = document.createElement('div');
  tempDiv.innerHTML = htmlContent || '';
  const text = tempDiv.textContent || tempDiv.innerText || '';
  const firstSentence = text.split('. ')[0];
  return firstSentence.endsWith('.') ? firstSentence : firstSentence + '.';
}



}