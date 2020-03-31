import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DrawingBoardComponent } from './drawing-board/drawing-board.component';
import { PlayerListComponent } from './player-list/player-list.component';
import { ChatComponentComponent } from './chat-component/chat-component.component';

@NgModule({
  declarations: [
    AppComponent,
    DrawingBoardComponent,
    PlayerListComponent,
    ChatComponentComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
