import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PostService } from '../../services/post.service';
import { FormsModule } from '@angular/forms'; 
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-edit-post',
  imports: [FormsModule, CommonModule],
  templateUrl: './edit-post.component.html',
  styleUrls: ['./edit-post.component.css']  
})
export class EditPostComponent implements OnInit {  

  postId: string = '';
  post: any = {
    title: '',
    content: '',
    category: 'lifestyle'
  };

  constructor(
    private route: ActivatedRoute,
    private postService: PostService,
    private router: Router
  ) {}

  ngOnInit(): void {  
    this.postId = this.route.snapshot.paramMap.get('id') || '';
    if (this.postId) {
      this.postService.getPostById(this.postId).subscribe({
        next: data => {
          this.post = data;
        },
        error: err => console.error('Failed to load post:', err)
      });
    }
  }

  saveChanges(): void {
    console.log('Save button clicked. Post data:', this.post); 
    this.postService.updatePost(this.postId, this.post).subscribe({
      next: () => {
        alert('Post updated successfully');
        this.router.navigate(['/profile']);
      },
      error: err => console.error('Update failed:', err)
    });
  }
}
