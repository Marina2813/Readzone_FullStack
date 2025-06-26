import { Component } from '@angular/core';
import { PostService } from '../../services/post.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../components/navbar/navbar.component';

@Component({
  standalone: true,
  imports: [CommonModule, NavbarComponent],
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})

export class ProfileComponent {
  userPosts: any[] = [];
  user: any;
  userName: string = '';

  constructor(
    private postService: PostService,
    private authService: AuthService,
    private router: Router
  ) {}



  ngOnInit(): void {
    this.loadUserPosts();
    this.loadUsername();
    
  }
  
  loadUsername(): void {
    const userId = this.authService.getUserId();
    if (userId) {
      this.authService.getUsernameById(userId).subscribe({
        next: (res) => {
          this.userName = res.username;
        },
        error: (err) => {
          console.error('Failed to fetch username:', err);
          this.userName = 'Unknown';
        }
      });
    }
  }


  loadUserPosts(): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    

    this.postService.getPostsByCurrentUser().subscribe({
      next: data => {
        console.log("Fetched posts:", data);

        const userId = this.authService.getUserId();
        console.log("Current userId:", userId); 

        data.forEach(post => console.log("Post userId:", post.userId));

        this.userPosts = data.filter(post => String(post.userId) === String(userId));
        console.log("Filtered userPosts:", this.userPosts);
      },
      error: err => console.error("Failed to fetch posts", err)
    });
    
  }

  deletePost(postId: string): void {
    if (confirm('Are you sure you want to delete this post?')) {
      this.postService.deletePost(postId).subscribe({
        next: () => {
          this.userPosts = this.userPosts.filter(p => p.postId !== postId);
        },
        error: err => console.error(err)
      });
    }
  }

  editPost(postId: string): void {
    this.router.navigate(['/edit', postId]);
  }

  goToPost(postId: string): void {
    this.router.navigate(['/post', postId]);
  }

  getTagline(htmlContent: string): string {
  const tempDiv = document.createElement('div');
  tempDiv.innerHTML = htmlContent || '';
  const text = tempDiv.textContent || tempDiv.innerText || '';
  const firstSentence = text.split('. ')[0];
  return firstSentence.endsWith('.') ? firstSentence : firstSentence + '.';
}

}
