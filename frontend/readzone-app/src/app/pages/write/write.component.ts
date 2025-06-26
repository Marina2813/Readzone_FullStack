import { Component, HostBinding} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxEditorModule } from 'ngx-editor';
import { Editor } from 'ngx-editor';

interface PostResponse {
  
  postId: string;
  title: string;
  content: string;
  category: string;
  userId: string;
  createdDate: string;
}

@Component({
  selector: 'app-write',
  standalone: true,
  templateUrl: './write.component.html',
  styleUrl: './write.component.css',
  imports: [
    FormsModule,
    NgxEditorModule,
  ],
})

export class WriteComponent {
  editor!: Editor;
  title = '';
  content = '';
  category = '';

  ngOnInit() {
    this.editor = new Editor();
  }

  ngOnDestroy() {
    this.editor.destroy();
  }

  constructor(private http: HttpClient, private router: Router) {}

  publishPost() {
    const token = localStorage.getItem('token');
    if (!token) {
      alert('You must be logged in to publish a post.');
      return;
    }
  
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    });

    const postData = {
      title: this.title,
      content: this.content,
      category: this.category,
      Comments: [],
      Likes: []
    };

    console.log('Attempting to publish post with:', postData);

     this.http.post<PostResponse>('https://localhost:7216/api/post', postData, { headers }).subscribe({
      next: (res) => {
        alert('Post published successfully!');
        console.log(res);
        this.title = '';
        this.content = '';
        this.category = '';

        const postId = res.postId;
        this.router.navigate(['/post', postId]); 
  },
      
      error: (err) => {
        console.error('Error publishing post:', err);
        console.log('Validation Errors:', err.error?.errors); 
        alert('Something went wrong');
      }
    });
  }

}
