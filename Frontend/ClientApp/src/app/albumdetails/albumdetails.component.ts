import { Component, OnInit, OnDestroy } from '@angular/core';
import { MusicstoreService } from '../services/musicstore.service';
import { ActivatedRoute } from '@angular/router';
import { AlbumDetail, UserAlbumOwnershipStatus } from '../models/albummodels';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-albumdetails',
  templateUrl: './albumdetails.component.html',
  styleUrls: ['./albumdetails.component.less'],
  //providers:[MusicstoreService]
})
export class AlbumdetailsComponent implements OnInit, OnDestroy {

  apiBaseUrl: string;
  album: AlbumDetail;
  ownedStatus: UserAlbumOwnershipStatus;

  private routeSub:Subscription;
  
  constructor(private service: MusicstoreService, private route: ActivatedRoute, private location: Location) {
    this.apiBaseUrl = service.baseUrl;
   }

  ngOnInit() {
    this.routeSub = this.route.params.subscribe( params => {
      if(params.id){
        let id: number = parseInt(params.id);
        this.service.getAlbumById(id).subscribe(res => this.album = res);
        this.service.getAlbumOwnershipStatus(id).subscribe(res => this.ownedStatus = res);
      }
    });
  }

  ngOnDestroy(){
    if(this.routeSub){
      this.routeSub.unsubscribe();
    }
  }

  backButtonOnClick(): void{
    this.location.back();
  }

  buyAlbumButtonOnClick():void{
    if(!this.ownedStatus.isOwned){
      this.service.purchaseAlbumForUser(this.album.id)
        .subscribe( () => {
          this.ownedStatus.isOwned = true;
        });
    }else{
      console.log('already owned');
    }
  }
}
