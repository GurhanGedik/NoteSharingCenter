﻿using NoteSharingCenter.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.DAL
{
    class FakeDataCreation : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            Users admin = new Users()
            {
                Name = "Gurhan",
                Surname = "GEDIK",
                Email = "gurhangedik@hotmail.com",
                ActiveteGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "admin",
                Password = "123",
                ProfileImageFilename="avatar.png",
                AboutMe= FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 5)),
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "Admin"
            };
            context.Users.Add(admin);

            Users user = new Users()
            {
                Name = "Ayten",
                Surname = "GEDIK",
                Email = "gurhangedik2@hotmail.com",
                ActiveteGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "user",
                Password = "123",
                ProfileImageFilename = "avatar.png",
                AboutMe = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 5)),
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "aytengedik"
            };
            context.Users.Add(user);

            for (int i = 0; i < 8; i++)
            {
                Users users = new Users()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActiveteGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = "user" + i,
                    Password = "123",
                    ProfileImageFilename = "avatar.png",
                    AboutMe = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 5)),
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = "user" + i
                };
                context.Users.Add(users);
            }

            context.SaveChanges();
            List<Users> userList = context.Users.ToList();

            //Add fake categories
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "Admin"
                };
                context.Categorys.Add(cat);

                //Add fake notes
                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 10); k++)
                {
                    Users owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 5)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        NoteImageFilename= "blog-11.jpg",
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    cat.Notes.Add(note);

                    //Add fake comments
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 5); j++)
                    {
                        Users commentOwner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = commentOwner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = commentOwner.Username
                        };
                        note.Comments.Add(comment);
                    }

                    //Add fake Likes
                    for (int l = 0; l < note.LikeCount; l++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[l]
                        };
                        note.Likes.Add(liked);
                    }
                }

            }
            context.SaveChanges();

        }
    }
}
