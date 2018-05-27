using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicStoreDemo.Database;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Database.Entities.Relationships;

namespace DatabaseSeeder
{
    public class ContentCreator
    {
		private readonly MusicStoreDbContext _context;

		public ContentCreator(MusicStoreDbContext context){
			_context = context;
		}
		public async Task CreateSeedContent()
		{
			DbArtist tempArtistResult = null;

			tempArtistResult = await CreateArtist("Beck", "Bio text...", new string[] { "Indie", "Electronic" }, null);

			await CreateAlbum(
				artistId: tempArtistResult.Id,
				title: "Modern Guilt",
				description: "",
				genres: new string[] { "Alternative" },
				producer: "Danger Mouse, Beck",
				label: "DGC Records",
				releaseDate: new DateTime(2008, 7, 8),
				price: 13.99,
				coverImagePath: "Images/Covers/Beck_ModernGuilt.jpg",
				tracks: new TrackStruct[]{
					new TrackStruct("Orphans", "3:15"),
					new TrackStruct("Gamma Ray", "2:57"),
					new TrackStruct("Chemtrails", "4:40"),
					new TrackStruct("Modern Guilt", "3:14"),
					new TrackStruct("Youthless", "3:00"),
					new TrackStruct("Walls", "2:22"),
					new TrackStruct("Replica", "3:25"),
					new TrackStruct("Soul of a Man", "2:36"),
					new TrackStruct("Profanity Prayers", "3:43"),
					new TrackStruct("Volcano", "4:26")
			});
            await CreateAlbum(
				artistId: tempArtistResult.Id,
				title: "Guero",
				description: "",
				genres: new string[] { "Electronic", "Hip Hop", "Rock" },
				producer: "Beck Hansen, Dust Brothers, Tony Hoffer",
				label: "Interscope Records",
				releaseDate: new DateTime(2005, 3, 21),
				price: 13.99,
				coverImagePath: "Images/Covers/beckguero.jpg",
				tracks: new TrackStruct[]{
                    new TrackStruct("E-Pro","3:22"),
                    new TrackStruct("Qué Onda Guero","3:29"),
                    new TrackStruct("Girl","3:30"),
                    new TrackStruct("Missing","4:44"),
                    new TrackStruct("lack Tambourine","2:46"),
                    new TrackStruct("Earthquake Weather","4:26"),
                    new TrackStruct("Hell Yes","3:18"),
                    new TrackStruct("Broken Drum","4:30"),
                    new TrackStruct("Scarecrow","4:16"),
                    new TrackStruct("Go It Alone","4:09"),
                    new TrackStruct("Farewell Ride","4:19"),
                    new TrackStruct("Rental Car","3:05"),
                    new TrackStruct("Emergency Exit", "4:01"),
                    new TrackStruct("Send A Message To Her","4:28"),
                    new TrackStruct("Chain Reaction","3:27"),
                    new TrackStruct("Clap Hands","3:19")
                });

			tempArtistResult = await CreateArtist("Fleetwood Mac", "Bio text...", new string[] { "Rock" }, null);

			await CreateAlbum(
				artistId: tempArtistResult.Id,
				title: "Rumours",
				description: "",
				genres: new string[] { "Rock" },
				producer: "Fleetwood Mac, Ken Caillat, Richard Dashut",
				label: "Warner Bros",
				releaseDate: new DateTime(1977, 2, 4),
				price: 13.99,
				coverImagePath: "Images/Covers/FMacRumours.png",
				tracks: new TrackStruct[]{
				new TrackStruct("Second Hand News", "2:56"),
				new TrackStruct("Dreams", "4:14"),
				new TrackStruct("Never Going Back Again", "2:14"),
				new TrackStruct("Dont Stop", "3:13"),
				new TrackStruct("Go Your Own Way", "3:13"),
				new TrackStruct("Songbird", "3:30"),
				new TrackStruct("The Chain", "4:30"),
				new TrackStruct("You Make Loving Fun", "3:31"),
				new TrackStruct("I Don't Want to Know", "3:15"),
				new TrackStruct("Oh Daddy", "3:56"),
				new TrackStruct("Gold Dust Woman", "4:56")
			});

			tempArtistResult = await CreateArtist("LCD Soundsystem", "", new string[] { "Dance-Punk", "Electronica", "Electronic Rock", "Alt-Rock" }, null);
			await CreateAlbum(
				artistId: tempArtistResult.Id,
				title: "Sound of Silver",
				description: "",
				genres: new string[] { "Dance-Punk", "Electronica", "Electronic Rock" },
				producer: "DFA, Capitol, EMI",
				label: "The DFA",
				releaseDate: new DateTime(2007, 3, 12),
				price: 13.99,
				coverImagePath: "Images/Covers/SoundOfSilver.jpg",
				tracks: new TrackStruct[]{
				new TrackStruct("Get Innocuous!", "7:11"),
				new TrackStruct("Time to Get Away", "4:11"),
				new TrackStruct("North American scum", "5:25"),
				new TrackStruct("Someone Great", "6:25"),
				new TrackStruct("All My Friends", "7:37"),
				new TrackStruct("Us v Them", "8:29"),
				new TrackStruct("Watch the Tapes", "3:55"),
				new TrackStruct("Sound of Silver", "7:07"),
				new TrackStruct("New York, I Love You but You're Bringing Me Down", "5:35")
			});

			tempArtistResult = await CreateArtist("Arcade Fire", "", new string[] { "Rock", "Indie-Rock" }, null);

			await CreateAlbum(
				artistId: tempArtistResult.Id,
				title: "Funeral",
				description: "",
				genres: new string[] { "Indie-Rock", "Rock" },
				producer: "Arcade Fire",
				label: "Merge",
				releaseDate: new DateTime(2004, 9, 14),
				price: 13.99,
				coverImagePath: "Images/Covers/afFuneral.jpg",
				tracks: new TrackStruct[]{
					new TrackStruct("Neighborhood #1 (Tunnels)","4:48"),
					new TrackStruct("Neighborhood #2 (Laïka)","3:32"),
					new TrackStruct("Une Année Sans Lumière", "3:41"),
					new TrackStruct("Neighborhood #3 (Power Out)","5:12"),
					new TrackStruct("Neighborhood #4 (7 Kettles)","4:49"),
					new TrackStruct("Crown Of Love","4:42"),
					new TrackStruct("Wake Up","5:35"),
					new TrackStruct("Haïti","4:07"),
					new TrackStruct("Rebellion(Lies)","5:12"),
					new TrackStruct("In The Backseat","6:20")
			});

			tempArtistResult = await CreateArtist("Pixies", "", new string[] { "Noise-Pop", "Alt-Rock" }, null);         
            await CreateAlbum(
                artistId: tempArtistResult.Id,
				title: "Doolittle",
                description: "",
				genres: new string[] { "Noise-Pop", "Alt-Rock" },
				producer: "Gil Norton",
				label: "4AD, Elektra",
                releaseDate: new DateTime(1998, 10, 23),
                price: 13.99,
				coverImagePath: "Images/Covers/Pixies-Doolittle.jpg",
                tracks: new TrackStruct[]{
    				new TrackStruct("Debaser","2:52"),
                    new TrackStruct("Tame","1:55"),
                    new TrackStruct("Wave Of Mutilation","2:04"),
                    new TrackStruct("I Bleed","2:34"),
                    new TrackStruct("Here Comes Your Man","3:21"),
                    new TrackStruct("Dead","2:21"),
                    new TrackStruct("Monkey Gone To Heaven","2:57"),
                    new TrackStruct("Mr.Grieves","2:05"),
                    new TrackStruct("Crackity Jones","1:24"),
                    new TrackStruct("La La Love You","2:43"),
                    new TrackStruct("No. 13 Baby","3:51"),
                    new TrackStruct("There Goes My Gun","1:49"),
                    new TrackStruct("Hey","3:31"),
                    new TrackStruct("Silver","2:25"),
                    new TrackStruct("Gouge Away","2:45")
            });

			tempArtistResult = await CreateArtist("Wilco", "", new string[] { "Alt-rock", "Indie-Rock", "Alternative country" }, null);         
            await CreateAlbum(
                artistId: tempArtistResult.Id,
				title: "Yankee Hotel Foxtrot",
                description: "",
				genres: new string[] {"Alt-rock", "Indie-Rock"},
				producer: "Wilco",
				label: "Nonesuch",
                releaseDate: new DateTime(1998, 10, 23),
                price: 13.99,
				coverImagePath: "Images/Covers/yankeehotelfoxtrot.jpg",
                tracks: new TrackStruct[]{
    				new TrackStruct("I Am Trying To Break Your Heart", "6:57"),
                    new TrackStruct("Kamera", "3:29"),
                    new TrackStruct("Radio Cure ", "5:08"),
                    new TrackStruct("War On War", "3:47"),
                    new TrackStruct("Jesus, Etc.", "3:50"),
                    new TrackStruct("Ashes Of American Flags", "4:43"),
                    new TrackStruct("Heavy Metal Drummer", "3:08"),
                    new TrackStruct("I'm The Man Who Loves You", "3:55"),
                    new TrackStruct("Pot Kettle Black", "4:00"),
                    new TrackStruct("Poor Places", "5:15"),
                    new TrackStruct("Reservations", "7:22")
            });         
			await CreateAlbum(
                artistId: tempArtistResult.Id,
                title: "Sky Blue Sky",
                description: "",
                genres: new string[] { "Alt-rock", "Indie-Rock" },
                producer: "Wilco",
				label: "Nonesuch",
                releaseDate: new DateTime(2007, 5, 15),
                price: 13.99,
                coverImagePath: "Images/Covers/skybluesky.jpg",
                tracks: new TrackStruct[]{
    				new TrackStruct("Either Way", "3:01"),
                    new TrackStruct("You Are My Face", "4:39"),
                    new TrackStruct("Impossible Germany", "5:58"),
                    new TrackStruct("Sky Blue Sky", "3:24"),
                    new TrackStruct("Side With The Seeds", "4:16"),
                    new TrackStruct("Shake It Off", "5:43"),
                    new TrackStruct("Please Be Patient With Me", "3:20"),
                    new TrackStruct("Hate It Here", "4:34"),
                    new TrackStruct("Leave Me(Like You Found Me)", "4:11"),
                    new TrackStruct("Walken", "4:28"),
                    new TrackStruct("What Light ", "3:36"),
                    new TrackStruct("On And On And On","4:02"),
            });

			tempArtistResult = await CreateArtist("Pearl Jam", "Bio text...", new string[] { "Grunge", "Rock" }, null);
			await CreateAlbum(
                artistId: tempArtistResult.Id,
                title: "Ten",
                description: "",
                genres: new string[] { "Alt-rock", "Grunge" },
				producer: "Rick Parashar, Pearl Jam",
                label: "Epic",
                releaseDate: new DateTime(1991, 8, 27),
                price: 9.99,
                coverImagePath: "Images/Covers/pjTen.jpg",
                tracks: new TrackStruct[]{
    				new TrackStruct("Once", "3:51"),
                    new TrackStruct("Even Flow", "4:53"),
                    new TrackStruct("Alive", "5:40"),
                    new TrackStruct("Why Go", "3:19"),
                    new TrackStruct("Black", "5:43"),
                    new TrackStruct("Jeremy", "5:18"),
                    new TrackStruct("Oceans", "2:41"),
                    new TrackStruct("Porch", "3:30"),
                    new TrackStruct("Garden", "4:58"),
                    new TrackStruct("Deep", "4:13"),
                    new TrackStruct("Release", "5:05"),
                    new TrackStruct("Master / Slave", "3:43"),
            });

			tempArtistResult = await CreateArtist("Graham Coxon", "Bio text...", new string[] { "Rock" }, null);
            await CreateAlbum(
                artistId: tempArtistResult.Id,
				title: "Love Travels At Illegal Speeds",
                description: "",
				genres: new string[] { "Rock", "Alternative Rock", "Brit Pop" },
				producer: "Stephen Street",
                label: "Parlophone",
                releaseDate: new DateTime(2006,3, 8),
                price: 9.99,
				coverImagePath: "Images/Covers/gc-ltais.jpeg",
                tracks: new TrackStruct[]{
    				new TrackStruct("Standing On My Own Again", "4:29"),
                    new TrackStruct("I Can't Look At Your Skin", "3:35"),
                    new TrackStruct("Don't Let Your Man Know", "2:54"),
                    new TrackStruct("Just A State Of Mind", "4:36"),
                    new TrackStruct("You & I", "3:42"),
                    new TrackStruct("Gimme Some Love", "2:32"),
                    new TrackStruct("I Don't Wanna Go Out", "4:17"),
                    new TrackStruct("Don't Believe Anything I Say", "5:26"),
                    new TrackStruct("Tell It Like It Is", "4:02"),
                    new TrackStruct("Flights To The Sea (Lovely Rain)", "3:25"),
                    new TrackStruct("What's He Got?", "3:42"),
                    new TrackStruct("You Always Let Me Down", "2:49"),
                    new TrackStruct("See A Better Day", "5:10"),
                    new TrackStruct("Click Click Click", "2:54"),
                    new TrackStruct("Livin'", "3:42"),
            });

            tempArtistResult = await CreateArtist("Yeah Yeah Yeahs", "Bio text...", new string[] { "Rock", "Electronic", "Alternative Rock"}, null);
            await CreateAlbum(
                artistId: tempArtistResult.Id,
				title: "It's Blitz!",
                description: "",
				genres: new string[] { "Rock", "Alternative Rock", "Electronic" },
				producer: "Nick Launay, David Sitek",
                label: "Dress Up, DGC, Interscope",
                releaseDate: new DateTime(2009,3, 6),
                price: 9.99,
				coverImagePath: "Images/Covers/itsblitz.jpeg",
                tracks: new TrackStruct[]{
                    new TrackStruct("Zero","4:26"),
                    new TrackStruct("Heads Will Roll","3:42"),
                    new TrackStruct("Soft Shock","3:53"),
                    new TrackStruct("Skeletons","5:02"),
                    new TrackStruct("Dull Life","4:08"),
                    new TrackStruct("Shame And Fortune","3:31"),
                    new TrackStruct("Runaway","5:13"),
                    new TrackStruct("Dragon Queen","4:02"),
                    new TrackStruct("Hysteric","3:52"),
                    new TrackStruct("Little Shadow","3:57"),
                });

                tempArtistResult = await CreateArtist("The Flaming Lips", "Bio text...", new string[] { "Experimental", "Indie Rock", "Psychedelic Rock"}, null);
                await CreateAlbum(
                    artistId: tempArtistResult.Id,
                    title: "The Soft Bulletin",
                    description: "",
                    genres: new string[] { "Experimental", "Indie Rock", "Psychedelic Rock" },
                    producer: "Dave Fridmann",
                    label: "Warner Bros",
                    releaseDate: new DateTime(1999,5, 17),
                    price: 9.99,
                    coverImagePath: "Images/Covers/flaminglipstsb.jpeg",
                    tracks: new TrackStruct[]{
                        new TrackStruct("Race For The Prize","4:18"),
                        new TrackStruct("A Spoonful Weighs A Ton","3:32"),
                        new TrackStruct("The Spark That Bled","5:55"),
                        new TrackStruct("Slow Motion","3:53"),
                        new TrackStruct("What Is The Light?","4:05"),
                        new TrackStruct("The Observer","4:10"),
                        new TrackStruct("Waitin' For A Superman","4:17"),
                        new TrackStruct("Suddenly Everything Has Changed","3:54"),
                        new TrackStruct("The Gash","4:02"),
                        new TrackStruct("Feeling Yourself Disintegrate","5:17"),
                        new TrackStruct("Sleeping On The Roof","3:10"),
                        new TrackStruct("Race For The Prize","4:09"),
                        new TrackStruct("Waitin' For A Superman","4:19"),
                        new TrackStruct("Buggin'","3:16")
                    });
                
                tempArtistResult = await CreateArtist("Pavement", "Bio text...", new string[] { "Indie Rock"}, null);
                await CreateAlbum(
                    artistId: tempArtistResult.Id,
                    title: "Crooked Rain, Crooked Rain",
                    description: "",
                    genres: new string[] { "Indie Rock" },
                    producer: "Pavement",
                    label: "Big Cat",
                    releaseDate: new DateTime(1999,5, 17),
                    price: 9.99,
                    coverImagePath: "Images/Covers/crookedrain.jpeg",
                    tracks: new TrackStruct[]{
                        new TrackStruct("Silence Kit","3:00"),
                        new TrackStruct("Elevate Me Later","2:51"),
                        new TrackStruct("Stop Breathin","4:27"),
                        new TrackStruct("Cut Your Hair","3:06"),
                        new TrackStruct("Newark Wilder","3:53"),
                        new TrackStruct("Unfair","2:33"),
                        new TrackStruct("Gold Soundz","2:39"),
                        new TrackStruct("5-4 = Unity","2:09"),
                        new TrackStruct("Range Life","4:54"),
                        new TrackStruct("Heaven Is A Truck","2:30"),
                        new TrackStruct("Hit The Plane Down","3:36"),
                        new TrackStruct("Fillmore Jive","6:38")
                    });
                
                tempArtistResult = await CreateArtist("Doves", "Bio text...", new string[] { "Indie Rock"}, null);
                await CreateAlbum(
                    artistId: tempArtistResult.Id,
                    title: "Lost Souls",
                    description: "",
                    genres: new string[] { "Brit Pop","Indie Rock" },
                    producer: "Doves; Steve Osborne",
                    label: "Heavenly Records",
                    releaseDate: new DateTime(2000,5, 3),
                    price: 9.99,
                    coverImagePath: "Images/Covers/lostsouls.jpeg",
                    tracks: new TrackStruct[]{
                        new TrackStruct("Firesuite","4:36"),
                        new TrackStruct("Here It Comes","4:50"),
                        new TrackStruct("Break Me Gently","4:38"),
                        new TrackStruct("Sea Song","6:12"),
                        new TrackStruct("Rise","5:38"),
                        new TrackStruct("Lost Souls","6:09"),
                        new TrackStruct("Melody Calls","3:27"),
                        new TrackStruct("Catch The Sun","4:49"),
                        new TrackStruct("The Man Who Told Everything","5:47"),
                        new TrackStruct("The Cedar Room","7:38"),
                        new TrackStruct("Reprise","1:45"),
                        new TrackStruct("A House","3:40")
                    });

                tempArtistResult = await CreateArtist("Queens of the Stone Age", "Bio text...", new string[] { "Rock", "Indie Rock"}, null);
                await CreateAlbum(
                    artistId: tempArtistResult.Id,
                    title: "Rated R",
                    description: "",
                    genres: new string[] { "Rock" },
                    producer: "Chris Goss, Joshua Homme",
                    label: "Heavenly Records",
                    releaseDate: new DateTime(2000,6, 5),
                    price: 9.99,
                    coverImagePath: "Images/Covers/qotsar.jpeg",
                    tracks: new TrackStruct[]{
                        new TrackStruct("Feel Good Hit Of The Summer","2:43"),
                        new TrackStruct("The Lost Art Of Keeping A Secret","3:36"),
                        new TrackStruct("Leg Of Lamb","2:48"),
                        new TrackStruct("Auto Pilot","4:01"),
                        new TrackStruct("Better Living Through Chemistry","5:49"),
                        new TrackStruct("Monsters In The Parasol","3:27"),
                        new TrackStruct("Quick And To The Pointless","1:42"),
                        new TrackStruct("In The Fade","3:51"),
                        new TrackStruct("Untitled","0:34"),
                        new TrackStruct("Tension Head","2:52"),
                        new TrackStruct("Lightning Song","2:07"),
                        new TrackStruct("I Think I Lost My Headache","8:40")
                    });


                tempArtistResult = await CreateArtist("The Go! Team", "Bio text...", new string[] { "Leftfield", "Abstract", "Electro", "Future Jazz"}, null);
                await CreateAlbum(
                    artistId: tempArtistResult.Id,
                    title: "Thunder, Lightning, Strike",
                    description: "",
                    genres: new string[] { "Leftfield", "Abstract", "Electro", "Future Jazz" },
                    producer: "The Go! Team, Gareth Parton",
                    label: "Memphis Industries",
                    releaseDate: new DateTime(2004,9, 13),
                    price: 9.99,
                    coverImagePath: "Images/Covers/thunderlightningstrike.jpeg",
                    tracks: new TrackStruct[]{
                        new TrackStruct("Panther Dash","2:42"),
                        new TrackStruct("Ladyflash","4:07"),
                        new TrackStruct("Feelgood By Numbers","1:56"),
                        new TrackStruct("The Power Is On","3:11"),
                        new TrackStruct("Get It Together","3:24"),
                        new TrackStruct("Junior Kickstart","3:30"),
                        new TrackStruct("Air Raid Gtr","0:41"),
                        new TrackStruct("Bottle Rocket","3:40"),
                        new TrackStruct("Friendship Update","3:58"),
                        new TrackStruct("Huddle Formation","3:09"),
                        new TrackStruct("Everyone's A V.I.P. To Someone","4:55")
                    });


            // randomize the item order in each group
            foreach(var group in _context.AlbumGroups){
                var items =  _context.AlbumGroupListPositions.Where(x=>x.GroupId == group.Id)
                    .AsEnumerable()
                    .OrderBy(x => Guid.NewGuid());
                int i = 1;
                foreach(var itm in items){
                    itm.PositionIndex = i;
                    i++;
                }
                await _context.SaveChangesAsync();
            }

		}

		private async Task<DbArtist> CreateArtist(string name, string bioText, string[] genres, string bioImagePath)
        {
			foreach(string genre in genres){
				await this.CreateGenre(genre);
			}
            var existing = await this._context.Artists.FirstOrDefaultAsync(a => a.Name == name);
            if (existing != null)
            {
                return existing;
            }
            var artist = new DbArtist()
            {
                Name = name,
                BioText = bioText,
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
                PublishStatus = DbPublishedStatus.PUBLISHED
            };
            foreach (string genre in genres)
            {
                var dbGenre = await _context.Genres.FirstAsync(x => x.Name == genre);
                if (dbGenre != null)
                {
                    artist.ArtistGenres.Add(new DbArtistGenre()
                    {
                        Genre = dbGenre,
                        CreatedUtc = DateTime.UtcNow
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(bioImagePath))
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), bioImagePath);
                if (File.Exists(fullPath))
                {

                    artist.BioImage = new DbImageResource()
                    {
                        Data = File.ReadAllBytes(fullPath),
                        MimeType = "image/png"
                    };
                }
            }

            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            return artist;
        }

        private async Task<DbAlbum> CreateAlbum(int artistId, string title, string description, string[] genres, string producer, DateTime releaseDate, string label, double price, string coverImagePath, TrackStruct[] tracks)
        {
			foreach (string genre in genres)
            {
                await this.CreateGenre(genre);
            }
            var existing = await this._context.Albums.FirstOrDefaultAsync(a => a.Title == title && a.ArtistId == artistId);
            if (existing != null)
            {
                return existing;
            }
            DbAlbum album = new DbAlbum()
            {
                Title = title,
                DescriptionText = description,
                Producer = producer,
                Label = label,
                ReleaseDate = releaseDate,
                ArtistId = artistId,
                Price = price,
                TotalDurationInSeconds = (tracks == null ? 0 : tracks.Select(x => x.DurationInSec).Sum()),
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
                PublishStatus = DbPublishedStatus.PUBLISHED
            };
			for (int i = 0; i < tracks.Length; i++)
            {
				var t = tracks[i];
                album.Tracks.Add(new DbTrack
                {
                    Title = t.Title,
                    CreatedUtc = DateTime.UtcNow,
                    DurationInSeconds = t.DurationInSec,
                    TrackNumber = (i+1),
                    UpdatedUtc = DateTime.UtcNow
                });
            }
            foreach (string genre in genres)
            {
                var dbGenre = await _context.Genres.FirstAsync(x => x.Name == genre);
                if (dbGenre != null)
                {
                    album.AlbumGenres.Add(new DbAlbumGenre()
                    {
                        Genre = dbGenre,
                        CreatedUtc = DateTime.UtcNow
                    });
                }
            }
			foreach(var g in _context.AlbumGroups){
				int totalItems = _context.AlbumGroupListPositions.Count(x => x.GroupId == g.Id);
				_context.AlbumGroupListPositions.Add(new DbAlbumGroupAlbumPosition
				{
					Album = album,
					CreatedUtc = DateTime.UtcNow,
					Group = g,
					PositionIndex = totalItems + 1
				});
			}
            if (!string.IsNullOrWhiteSpace(coverImagePath))
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), coverImagePath);
                if (File.Exists(fullPath))
                {

                    album.AlbumCoverImage = new DbImageResource()
                    {
                        Data = File.ReadAllBytes(fullPath),
                        MimeType = "image/png"
                    };
                }
            }
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
            return album;
        }

		internal async Task<List<DbGenre>> CreateGenres(params string[] names)
        {
            List<DbGenre> results = new List<DbGenre>();
            foreach (string genreName in names)
            {
                results.Add(await CreateGenre(genreName));
            }
            return results;
        }

        private async Task<DbGenre> CreateGenre(string name)
        {
            var dbGenre = await _context.Genres.FirstOrDefaultAsync(x => x.Name == name);
            if (dbGenre == null)
            {
                var newGenre = new DbGenre()
                {
                    Name = name,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                };
                _context.Genres.Add(newGenre);
                await _context.SaveChangesAsync();
                return newGenre;
            }
            else
            {
                return dbGenre;
            }
        }
    }
}
