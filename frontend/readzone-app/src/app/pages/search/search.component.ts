import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { AppModule } from '../../app.module';
import { FooterComponent } from '../../components/footer/footer.component';

@Component({
  selector: 'app-search-results',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  imports: [
    CommonModule,
    RouterModule,
    NavbarComponent, 
    AppModule 
  ],
  
})
export class SearchComponent implements OnInit {
  query = '';
  results: any[] = [];
  loading = true;

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.query = params['q'] || '';
      if (this.query.trim()) {
        this.searchPosts();
      }
    });
  }

  searchPosts(): void {
    this.http.get<any[]>(`https://localhost:7216/api/post/search?query=${encodeURIComponent(this.query)}`).subscribe(posts => {
      const lower = this.query.toLowerCase();
      this.results = posts.filter(post =>
        post.title.toLowerCase().includes(lower) ||
        post.content?.toLowerCase().includes(lower)
      );
    });
  }
  
  }

