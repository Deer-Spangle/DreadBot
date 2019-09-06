﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DreadBot
{
    partial class Methods
    {
        /// <summary>
        /// Returns a User object which contains the user data about the bot. In Most cases, you wont need this.
        /// </summary>
        /// <returns></returns>
        public static Result<User> getMe()
        {
            return sendRequest<User>(Method.getMe);
        }

        /// <summary>
        /// Sends a message to a Chat, Group, Channel, or User. Returns the Result<Message> object on success.
        /// </summary>
        /// <param name="chat_id">The Id number od the chat to send a message. Can be a User, Channel, Or group. Cannot be a bot.</param>
        /// <param name="text">The text to send to the Chat. Character Limit of 4096.</param>
        /// <param name="parse_mode">Makrkdown, HTML, or Empty. Tells telegram how to parse special markdown flags in the text. Makrdown by default.</param>
        /// <param name="keyboard">InlineKeyboardMarkup Object. Pass a built Keyboard object in here to include it in your messages.</param>
        /// <returns></returns>
        public static Result<Message> sendMessage(long chatId, string text, string parse_mode = "markdown", InlineKeyboardMarkup keyboard = null)
        {
            SendMessageRequest smr = new SendMessageRequest()
            {
                chat_id = chatId,
                text = text,
                parse_mode = parse_mode
            };

            if (keyboard != null) { smr.reply_markup = keyboard; }
            Result<Message> result = null;
            result = sendRequest<Message>(Method.sendMessage, buildRequest<SendMessageRequest>(smr));
            isOk<Message>(result);
            return result;
        }

        /// <summary>
        /// Sends a message to a Chat in response to another message, Channel, Group, or User. Returns the Result<Message> object on success.
        /// </summary>
        /// <param name="args">Pass a filled int FowardMessageRequest object to forward a message to a Channel, Group or User.</param>
        /// <returns></returns>
        public static Result<Message> forwardMessage(ForwardMessageRequest args)
        {
            Result<Message> result = null;
            if (args == null) { result = sendRequest<Message>(Method.forwardMessage); }
            else { result = sendRequest<Message>(Method.getUpdates, buildRequest<ForwardMessageRequest>(args)); }
            return result;
        }

        /// <summary>
        /// Sends a message to a Chat in response to another message, Channel, Group, or User. Returns the Result<Message> object on success.
        /// </summary>
        /// <param name="chat_id">The Id number od the chat to send a Photo. Can be a User, Channel, Or group. Cannot be a bot.</param>
        /// <param name="fileData">The StreamContet object for the raw image file to upload. Cannot be larger than 20MB.</param>
        /// <param name="filename">Telegram requires a file name for images. It can be anything, must end in jpg, png, or jpeg</param>
        /// <param name="caption">The text to send to the Chat, as part of the image. Character Limit of 4096.
        /// <param name="parse_mode">Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in the media caption. Makrdown by default.</param>
        /// <param name="disable_notification">If True, Sends the message silently. Users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id" >The ID of the message you want this Photo messgae to be in response of. Setting this to 0 will not send it as a reply.</param>
        /// <param name="keyboard">InlineKeyboardMarkup Object. Pass a built Keyboard object in here to include it in your messages.</param>
        /// <returns></returns>
        public static Result<Message> sendPhoto(long chatId, StreamContent fileData, string filename, string caption, string parse_mode = "markdown", bool disable_notification = false, int reply_to_message_id = 0, InlineKeyboardMarkup keyboard = null)
        {
            SendPhotoDataRequest spr = new SendPhotoDataRequest()
            {
                chat_id = chatId,
                caption = caption,
                parse_mode = parse_mode,
            };
            using (var form = new MultipartFormDataContent())
            {
                form.Add(new StringContent(chatId.ToString(), Encoding.UTF8), "chat_id");
                form.Add(fileData, "photo", filename);
                if (caption != null)
                {
                    form.Add(new StringContent(caption.ToString(), Encoding.UTF8), "caption");
                }
                if (parse_mode != null)
                {
                    form.Add(new StringContent(parse_mode.ToString(), Encoding.UTF8), "parse_mode");
                }
                if (disable_notification)
                {
                    form.Add(new StringContent(disable_notification.ToString(), Encoding.UTF8), "disable_notification");
                }
                if (reply_to_message_id != 0)
                {
                    form.Add(new StringContent(reply_to_message_id.ToString(), Encoding.UTF8), "reply_to_message_id");
                }
                if (keyboard != null)
                {
                    string payload1 = buildRequest<SendPhotoDataRequest>(spr);
                    form.Add(new StringContent(payload1, Encoding.UTF8), "reply_markup");
                }
                Result<Message> result = sendRequest<Message>(Method.sendPhoto, "", "", form);
                isOk(result);
                return result;
            }
        }
        /// <summary>
        /// Sends a message to a Chat in response to another message, Channel, Group, or User. Returns the Result<Message> object on success.
        /// </summary>
        /// <param name="chat_id">The Id number od the chat to send a Photo. Can be a User, Channel, Or group. Cannot be a bot.</param>
        /// <param name="url">The Direct URL link to the image for Telegram to send to the chat. Cannot be larger than 5MB.</param>
        /// <param name="caption">The text to send to the Chat, as part of the image. Character Limit of 4096.
        /// <param name="parse_mode">Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in the media caption. Makrdown by default.</param>
        /// <param name="disable_notification">If True, Sends the message silently. Users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id" >The ID of the message you want this Photo messgae to be in response of. Setting this to 0 will not send it as a reply.</param>
        /// <param name="keyboard">InlineKeyboardMarkup Object. Pass a built Keyboard object in here to include it in your messages.</param>
        /// <returns></returns>
        public static Result<Message> sendPhoto(long chatId, string url, string caption, string parse_mode = "markdown", bool disable_notification = false, int reply_to_message_id = 0, InlineKeyboardMarkup keyboard = null)
        {
            SendPhotoUrlRequest spr = new SendPhotoUrlRequest()
            {
                photo_url = url,
                chat_id = chatId,
                caption = caption,
                parse_mode = parse_mode,
            };
            if (disable_notification) { spr.disable_notification = true; }
            if (reply_to_message_id != 0) { spr.reply_to_message_id = reply_to_message_id; }
            if (keyboard != null) { spr.reply_markup = keyboard; }
            Result<Message> result;
            result = sendRequest<Message>(Method.sendPhoto, buildRequest<SendPhotoUrlRequest>(spr));
            isOk(result);
            return result;
        }

        /// <summary>
        /// Sends a message to a Chat in response to another message, Channel, Group, or User. Returns the Result<Message> object on success.
        /// </summary>
        /// <param name="chat_id">The Id number od the chat to send a Audio file. Can be a User, Channel, Or group. Cannot be a bot.</param>
        /// <param name="fileData">The StreamContet object for the raw audio file to upload. Cannot be larger than 20MB.</param>
        /// <param name="filename">Telegram requires a file name for files. It can be anything, must end in the file ext that matches the type of audio file you're sending.</param>
        /// <param name="caption">The text to send to the Chat, as part of the audio file. Character Limit of 4096.
        /// <param name="parse_mode">Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in the media caption. Makrdown by default.</param>
        /// <param name="duration">The length of the audio file in seconds.</param>
        /// <param name="performer">The artist of of audio clip (If its music) Optional.</param>
        /// <param name="title">The title of of audio clip (If its music) Optional.</param>
        /// <param name="disable_notification">If True, Sends the message silently. Users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id" >The ID of the message you want this Photo messgae to be in response of. Setting this to 0 will not send it as a reply.</param>
        /// <param name="keyboard">InlineKeyboardMarkup Object. Pass a built Keyboard object in here to include it in your messages.</param>
        /// <returns></returns>
        public static Result<Message> sendAudio(long chatId, StreamContent fileData, string filename, string caption, string parse_mode = "markdown", int duration = 0, string performer = "", string title = "", bool disable_notification = false, int reply_to_message_id = 0, InlineKeyboardMarkup keyboard = null)
        {
            SendAudioRequest spr = new SendAudioRequest()
            {
                chat_id = chatId,
                caption = caption,
                parse_mode = parse_mode,
            };
            using (var form = new MultipartFormDataContent())
            {
                form.Add(new StringContent(chatId.ToString(), Encoding.UTF8), "chat_id");
                form.Add(fileData, "audio", filename);
                if (caption != null)
                {
                    form.Add(new StringContent(caption.ToString(), Encoding.UTF8), "caption");
                }
                if (parse_mode != null)
                {
                    form.Add(new StringContent(parse_mode.ToString(), Encoding.UTF8), "parse_mode");
                }
                if (duration != 0)
                {
                    form.Add(new StringContent(duration.ToString(), Encoding.UTF8), "duration");
                }
                if (performer != "")
                {
                    form.Add(new StringContent(performer.ToString(), Encoding.UTF8), "performer");
                }
                if (title != "")
                {
                    form.Add(new StringContent(title.ToString(), Encoding.UTF8), "title");
                }
                if (disable_notification)
                {
                    form.Add(new StringContent(disable_notification.ToString(), Encoding.UTF8), "disable_notification");
                }
                if (reply_to_message_id != 0)
                {
                    form.Add(new StringContent(reply_to_message_id.ToString(), Encoding.UTF8), "reply_to_message_id");
                }
                if (keyboard != null)
                {
                    string payload1 = buildRequest<SendPhotoDataRequest>(spr);
                    form.Add(new StringContent(payload1, Encoding.UTF8), "reply_markup");
                }
                Result<Message> result = sendRequest<Message>(Method.sendAudio, "", "", form);
                isOk(result);
                return result;
            }
        }

        /// <summary>
        /// Use this method to send general files. On success, the sent Result<Message> is returned. Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the future.
        /// </summary>
        /// <param name="chat_id">The Id number od the chat to send a Photo. Can be a User, Channel, Or group. Cannot be a bot.</param>
        /// <param name="fileData">The StreamContet object for the raw image file to upload. Cannot be larger than 20MB.</param>
        /// <param name="thumb">Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side. The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail‘s width and height should not exceed 320.</param>
        /// <param name="caption">Document caption (may also be used when resending documents by file_id), 0-1024 characters.
        /// <param name="parse_mode">Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in the media caption. Makrdown by default.</param>
        /// <param name="disable_notification">If True, Sends the message silently. Users will receive a notification with no sound.</param>
        /// <param name="reply_to_message_id" >The ID of the message you want this Photo messgae to be in response of. Setting this to 0 will not send it as a reply.</param>
        /// <param name="keyboard">InlineKeyboardMarkup Object. Pass a built Keyboard object in here to include it in your messages.</param>
        /// <returns></returns>
        public static Result<Message> sendDocument(long chatId, StreamContent fileData, string caption, StreamContent thumb = null, string parse_mode = "markdown", bool disable_notification = false, int reply_to_message_id = 0, InlineKeyboardMarkup keyboard = null)
        {
            SendDocumentDataRequest sddr = new SendDocumentDataRequest()
            {
                chat_id = chatId,
                caption = caption,
                parse_mode = parse_mode,
            };
            using (var form = new MultipartFormDataContent())
            {
                form.Add(fileData, "document");
                if (thumb != null) { form.Add(thumb, "thumb"); }
                form.Add(new StringContent(chatId.ToString(), Encoding.UTF8), "chat_id");

                if (caption != null)
                {
                    form.Add(new StringContent(caption.ToString(), Encoding.UTF8), "caption");
                }
                if (parse_mode != null)
                {
                    form.Add(new StringContent(parse_mode.ToString(), Encoding.UTF8), "parse_mode");
                }
                if (disable_notification)
                {
                    form.Add(new StringContent(disable_notification.ToString(), Encoding.UTF8), "disable_notification");
                }
                if (reply_to_message_id != 0)
                {
                    form.Add(new StringContent(reply_to_message_id.ToString(), Encoding.UTF8), "reply_to_message_id");
                }
                if (keyboard != null)
                {
                    string payload1 = buildRequest<SendDocumentDataRequest>(sddr);
                    form.Add(new StringContent(payload1, Encoding.UTF8), "reply_markup");
                }
                Result<Message> result = sendRequest<Message>(Method.sendDocument, "", "", form);
                isOk(result);
                return result;
            }
        }

        public static Result<Message> sendVideo()
        {
            return null;
        }
        public static Result<Message> sendVoice()
        {
            return null;
        }
        public static Result<Message> sendVideoNote()
        {
            return null;
        }
        public static Result<Message[]> sendMediaGroup()
        {
            return null;
        }
        public static Result<Message> sendLocation()
        {
            return null;
        }
        public static Result<Message> editMessageLiveLocation() //Can apprently return a bool as well. (FUCK THIS)
        {
            return null;
        }
        public static Result<Message> stopMessageLiveLocation() //Can apprently return a bool as well.
        {
            return null;
        }
        public static Result<Message> sendVenue()
        {
            return null;
        }
        public static Result<Message> sendContact()
        {
            return null;
        }

        public static Result<Message> sendPoll()
        {
            return null;
        }

        public static Result<bool> sendChatAction()
        {
            return null;
        }

        /// <summary>
        /// Returns a UserProfilePhotos object which contains an array of PhotoSize objects. These can be used to request the images from Telegram as Files to downlaod.
        /// </summary>
        /// <param name="user_id">The Numeric long that represents a user or bot</param>
        /// <param name="offset">The int offset from which to begin returning an image array.</param>
        /// <param name="limit">Limit the number of images returned. Limit zero assumes no limit.</param>
        /// <returns></returns>
        public static Result<UserProfilePhotos> getUserProfilePhotos(long user_id, int offset = 0, int limit = 0)
        {
            UserProfilePhotosRequest uppr = new UserProfilePhotosRequest() { user_id = user_id };
            if (offset > 0) { uppr.offset = offset; }
            if (limit > 0) { uppr.limit = limit; }
            Result<UserProfilePhotos> result = null;
            result = sendRequest<UserProfilePhotos>(Method.getUserProfilePhotos, buildRequest<UserProfilePhotosRequest>(uppr));
            isOk(result);
            return result;
        }
        /// <summary>
        /// Use this method to get basic info about a file and prepare it for downloading. For the moment, bots can download files of up to 20MB in size. On success, a File object is returned. The file can then be downloaded via the downloadFile() Method. It is guaranteed that the link will be valid for at least 1 hour. When the link expires, a new one can be requested by calling getFile again.
        /// </summary>
        /// <param name="file_id">Use this to provide the file id of the File on Telegram you want infor for or to download.</param>
        /// <returns></returns>
        public static Result<File> getFile(string file_id)
        {
            GetFileRequest gfr = new GetFileRequest() { file_id = file_id };
            Result<File> result = null;
            result = sendRequest<File>(Method.getFile, buildRequest<GetFileRequest>(gfr));
            isOk(result);
            return result;
        }

        /// <summary>
        /// Removes the user from the specified group, then removes the ban entry.
        /// </summary>
        /// <param name="chatId">Represents a group or channel id. You cannot use a bot or user ID here.</param>
        /// <param name="userId">Represents a user or bot to kick.</param>
        /// <returns></returns>
        public static Result<bool> kickChatMember(long chatId, long userId)
        {
            KickChatMemberRequest kcmr = new KickChatMemberRequest()
            {
                chat_id = chatId,
                user_id = userId,
            };
            string Payload = buildRequest<KickChatMemberRequest>(kcmr);
            Result<bool> result = null;
            Result<bool> unbanResult = null;
            result = sendRequest<bool>(Method.kickChatMember, Payload);
            isOk(result);

            if (result.ok)
            {
                unbanResult = sendRequest<bool>(Method.unbanChatMember, Payload);
            }
            else return result;
            return unbanResult;
        }

        /// <summary>
        /// This unbans the specified user in the specified chat.
        /// </summary>
        /// <param name="chatId">The Numeric Long that represents a group or channel ID. You cannot use a bot or user ID here.</param>
        /// <param name="userId">The Numeric long that represents a user or bot to unban.</param>
        /// <returns></returns>
        public static Result<bool> unbanChatMember(long chat_id, long user_id)
        {
            KickChatMemberRequest kcmr = new KickChatMemberRequest()
            {
                chat_id = chat_id,
                user_id = user_id
            };
            Result<bool> unbanResult = null;
            unbanResult = sendRequest<bool>(Method.unbanChatMember, buildRequest<KickChatMemberRequest>(kcmr));
            isOk(unbanResult);
            return unbanResult;
        }
        public static Result<bool> restrictChatMember()
        {
            return null;
        }
        public static Result<bool> promoteChatMember()
        {
            return null;
        }
        public static Result<bool> setChatPermissions()
        {
            return null;
        }
        /// <summary>
        /// Generates a new Invitelink and returns it on success.
        /// </summary>
        /// <param name="chat_id">The Id number of the chat get the invite from. Can be a Channel Or group. Cannot be a bot or User.</param>
        /// <returns></returns>
        public static Result<string> exportChatInviteLink(long chat_id)
        {
            GetChatRequest gcr = new GetChatRequest() { chat_id = chat_id };
            Result<string> result = null;
            result = sendRequest<string>(Method.exportChatInviteLink, buildRequest<KickChatMemberRequest>(gcr));
            isOk(result);
            return result;
        }

        public static Result<bool> setChatPhoto()
        {
            return null;
        }
        public static Result<bool> deleteChatPhoto()
        {
            return null;
        }
        public static Result<bool> setChatTitle()
        {
            return null;
        }
        public static Result<bool> setChatDescription()
        {
            return null;
        }
        public static Result<bool> pinChatMessage()
        {
            return null;
        }
        public static Result<bool> unpinChatMessage()
        {
            return null;
        }
        /// <summary>
        /// Force the bot to leave the specified Channel or Group. Result<bool> object that equals true on success.
        /// </summary>
        /// <param name="chat_id">The Id number of the chat to force the bot to leave. Can be a Channel Or group. Cannot be a bot or User.</param>
        /// <returns></returns>
        public static Result<bool> leaveChat(long chat_id)
        {
            GetChatRequest gcr = new GetChatRequest() { chat_id = chat_id };
            Result<bool> result = null;
            result = sendRequest<bool>(Method.leaveChat, buildRequest<GetChatRequest>(gcr));
            isOk(result);
            return result;
        }
        /// <summary>
        /// Returns a Chat Object containing information about the chat. Result<Chat> object on success.
        /// </summary>
        /// <param name="chat_id">The Id number of the chat to get info for. Can be a Channel Or group. Cannot be a bot or User.</param>
        /// <returns></returns>
        public static Result<Chat> getChat(long chat_id)
        {
            GetChatRequest gcr = new GetChatRequest() { chat_id = chat_id };
            Result<Chat> result = null;
            result = sendRequest<Chat>(Method.getChat, buildRequest<GetChatRequest>(gcr));
            isOk(result);
            return result;
        }
        /// <summary>
        /// Gets an array of ChatMember[] which contains the list of permissions for each Member. Result<ChatMember> object on success.
        /// </summary>
        /// <param name="chat_id">The Id number of the chat to get info for. Can be a Channel Or group. Cannot be a bot or User.</param>
        /// <returns></returns>
        public static Result<ChatMember[]> getChatAdministrators(long chat_id)
        {
            GetChatRequest gcr = new GetChatRequest() { chat_id = chat_id };
            Result<ChatMember[]> result = null;
            result = sendRequest<ChatMember[]>(Method.getChatAdministrators, buildRequest<GetChatRequest>(gcr));
            isOk(result);
            return result;
        }
        /// <summary>
        /// Gets the total number of members in a chat. Result<int> object on success.
        /// </summary>
        /// <param name="chat_id">The Id number of the chat to get info for. Can be a Channel Or group. Cannot be a bot or User.</param>
        /// <returns></returns>
        public static Result<int> getChatMembersCount(long chat_id)
        {
            GetChatRequest gcr = new GetChatRequest() { chat_id = chat_id };
            Result<int> result = null;
            result = sendRequest<int>(Method.getChatMembersCount, buildRequest<GetChatRequest>(gcr));
            isOk(result);
            return result;
        }

        /// <summary>
        /// Gets a single ChatMember object which contains the list of permissions for that member. Result<ChatMember> object on success.
        /// </summary>
        /// <param name="chat_id">The Id number of the chat the user is in. Can be a Channel Or group. Cannot be a bot or User.</param>
        /// <param name="user_id">The Id number of the user to get info of. Can be a bot or User. Cannot be a Channel Or group.</param>
        /// <returns></returns>
        public static Result<ChatMember> getChatMember(long chatId, long userId)
        {
            ChatMemberRequest gcmr = new ChatMemberRequest()
            {
                chat_id = chatId,
                user_id = userId
            };
            Result<ChatMember> result;
            result = sendRequest<ChatMember>(Method.getChatMember, buildRequest<ChatMemberRequest>(gcmr));
            isOk(result);
            return result;
        }
        public static Result<bool> setChatStickerSet()
        {
            return null;
        }
        public static Result<bool> deleteChatStickerSet()
        {
            return null;
        }
        /// <summary>
        /// Answers a Callback (Keyboard a Button Push) and optionally sends a message prompt to the user confirming the action. Result<bool> = true on success.
        /// </summary>
        /// <param name="Callback">The Call back ID provided by telegram in The CallBack object.</param>
        /// <param name="text">Optional text to show the user.</param>
        /// <param name="url">URL that will be opened by the user's client. If you have created a Game and accepted the conditions via @Botfather, specify the URL that opens your game – note that this will only work if the query comes from a callback_game button.</param>
        /// <param name="cache_time">The maximum amount of time in seconds that the result of the callback query may be cached client-side. Telegram apps will support caching starting in version 3.14. Defaults to 0.</param>
        /// <returns></returns>
        public static Result<bool> answerCallbackQuery(string Callback, string text = "", bool show_alert = false, string url = "", int cache_time = 0)
        {
            AnswerCallBackRequest acbr = new AnswerCallBackRequest()
            {
                callback_query_id = Callback
            };

            if (text != "") { acbr.text = text; }
            acbr.show_alert = show_alert;
            if (url != "") { acbr.url = url; }
            if (cache_time != 0) { acbr.cache_time = cache_time; }

            Result<bool> result = null;
            result = sendRequest<bool>(Method.answerCallbackQuery, buildRequest<AnswerCallBackRequest>(acbr));
            isOk(result);
            return result;
        }
    }
}
