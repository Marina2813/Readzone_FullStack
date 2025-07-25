import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  featuredPosts: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadFeaturedPosts();
  }

  loadFeaturedPosts(): void {
    this.http.get<any[]>('http://localhost:5213/api/post').subscribe({
      next: (posts) => {
        this.featuredPosts = posts.slice(0, 6); 
      },
      error: (err) => {
        console.error('Error loading posts:', err);
      }
    });
  }

}
