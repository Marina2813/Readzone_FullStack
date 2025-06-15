import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './auth.interceptor'; 

import { RouterModule, Routes } from '@angular/router';
import { LandingComponent } from './pages/landing/landing.component';
import { LoginComponent } from './pages/login/login.component';
import { SignupComponent } from './pages/signup/signup.component';
import { HomeComponent } from './pages/home/home.component';
import { WriteComponent } from './pages/write/write.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { PostDetailComponent } from './pages/post-detail/post-detail.component';
import { EditPostComponent } from './pages/edit-post/edit-post.component';
import { ForgotPasswordComponent } from './pages/forgot-password/forgot-password.component';
import { AllPostComponent } from './pages/all-post/all-post.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';

const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'home', component: HomeComponent },
  { path: 'write', component: WriteComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'post/:id', component: PostDetailComponent },
  { path: 'edit/:id', component: EditPostComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'all-post', component: AllPostComponent},
  { path: '**', redirectTo: '' }, 
];

@NgModule({
  declarations: [
    AppComponent,
    
    LandingComponent,
    HomeComponent,
    ProfileComponent,
    PostDetailComponent,
    EditPostComponent,
    ForgotPasswordComponent,
    AllPostComponent,
    NavbarComponent,
    FooterComponent
  ],
  imports: [
    FormsModule,
    BrowserModule,
    AppRoutingModule,
    LoginComponent,
    SignupComponent,
    HttpClientModule,
    [RouterModule.forRoot(routes)]
  ],
  exports: [RouterModule],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
