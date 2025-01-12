using BitesByte_API.Model;
using System;

namespace BitesByte_API.Service
{
    public interface IUserService
    {
        User RegisterUser(UserDTO user );
        bool LoginUser(LoginUserDTO user);
    }
    public class UserService : IUserService
    {
        private readonly BitesByteDbContext bitesByteDbContext;
        public UserService(BitesByteDbContext _bitesByteDbContext)
        {
            this.bitesByteDbContext = _bitesByteDbContext;
        }

        public User RegisterUser(UserDTO user)
        {
            try
            {
                User newuser = new User();
                if (IsValidInputforRegisterUser(user))
                {
                    newuser.Name = user.Name;
                    newuser.Email = user.Email;
                    newuser.FoodAllergies = user.FoodAllergies;
                    newuser.Password = user.Password;
                    newuser.AvgExerciseDuration = user.AvgExerciseDuration;
                    newuser.GoalWeight = user.GoalWeight;
                    newuser.GoalBodyFat = user.GoalBodyFat;

                    bitesByteDbContext.Users.Add(newuser);
                    bitesByteDbContext.SaveChanges();

                    return newuser;
                }


                return newuser;
            }
            catch (Exception) { 
              throw;
            }

        }

        public bool LoginUser(LoginUserDTO user)
        {
            try
            {
                bool res = false;

                if (CheckIfRegisteredUser(user))
                {
                    res = true;
                }

                return res;
            }
            catch(Exception) {
                throw;
            }
                

        }

        public bool CheckIfRegisteredUser(LoginUserDTO user)
        {
            if(user == null) return false;

            if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty( user.Password) ) { 
               
                User registereduser = bitesByteDbContext.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
                if (registereduser != null && !string.IsNullOrEmpty(user.Email)) {
                    return true;
                }
            }

            return false;
        }

        public bool IsValidInputforRegisterUser(UserDTO user)
        {
            bool isValid = false;
            if (user != null && !string.IsNullOrEmpty(user.Name) 
                && !string.IsNullOrEmpty(user.Email) 
                && !string.IsNullOrEmpty(user.Password)
                &&  !string.IsNullOrEmpty(user.ConfirmPassword) 
                && user.Password == user.ConfirmPassword  
                )
            {
                return true;
            }
            return isValid;
        }
    }
}
