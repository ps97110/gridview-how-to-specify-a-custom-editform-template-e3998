using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.SessionState;

namespace Sample.Models {
    public class User {
        int id;
        string nickname;
        string avatarUrl;

        public User() {
            ID = 0;
            NickName = string.Empty;
            AvatarUrl = string.Empty;
        }
        
        [Key]
        public int ID {
            get { return id; }
            set { id = value; }
        }
        [Required(ErrorMessage = "NickName is required")]
        public string NickName {
            get { return nickname; }
            set { nickname = value; }
        }
        public string AvatarUrl {
            get { return avatarUrl; }
            set { avatarUrl = value; }
        }
        public void Assign(User source) {
            NickName = source.NickName;
            AvatarUrl = source.AvatarUrl;
        }
    }

    public static class UserProvider {
        const string Key = "UserProvider";

        static HttpSessionState Session { get { return HttpContext.Current.Session; } }
        static List<User> Data {
            get {
                if(Session[Key] == null)
                    Restore();
                return Session[Key] as List<User>;
            }
        }

        public static IEnumerable<User> Select() {
            return Data;
        }
        public static void Insert(User item) {
            item.ID = Data.Count + 1;
            Data.Add(item);
        }
        public static void Update(User item) {
            User storedItem = FindItem(item.ID);
            storedItem.Assign(item);
        }
        public static void Delete(User item) {
            User storedItem = FindItem(item.ID);
            Data.Remove(storedItem);
        }
        public static void Restore() {
            Session[Key] = new List<User>();
        }
        static User FindItem(int id) {
            foreach(User item in Data) {
                if (item.ID == id)
                    return item;
            }
            return null;
        }
    }
}