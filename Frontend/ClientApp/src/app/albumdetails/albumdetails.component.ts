import { Component, OnInit } from '@angular/core';
import { MusicstoreService } from '../services/musicstore.service';
import { ActivatedRoute } from '@angular/router';
import { AlbumDetail, UserAlbumOwnershipStatus } from '../models/albummodels';
import { Location } from '@angular/common';

@Component({
  selector: 'app-albumdetails',
  templateUrl: './albumdetails.component.html',
  styleUrls: ['./albumdetails.component.less'],
  //providers:[MusicstoreService]
})
export class AlbumdetailsComponent implements OnInit {

  album: AlbumDetail;
  ownedStatus: UserAlbumOwnershipStatus;

  constructor(private service: MusicstoreService, private route: ActivatedRoute, private location: Location) { }

  ngOnInit() {
    let idParam = this.route.snapshot.paramMap.get('id');
    if(idParam){
      let id: number = parseInt(idParam);
      this.service.getAlbumById(id).subscribe(res => this.album = res);
      this.service.getAlbumOwnershipStatus(id).subscribe(res => this.ownedStatus = res);
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
