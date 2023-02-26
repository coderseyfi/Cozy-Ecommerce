function chatBox() {
    this.context = {
        inbox: [],
    };

    this.receiveMessage = (inboxMessage) => {
        let anotherMessages = this.context.inbox.filter(
            (i) => i.FriendId != inboxMessage.FriendId
        );

        anotherMessages.unshift(inboxMessage);
        this.context.inbox = anotherMessages;

        console.log(this.context.inbox);

        this.reloadInbox();
    }

    this.reloadInbox = () => {
        let messageList = document.querySelector("#messagelist");

        console.log(messageList);

        if (messageList == undefined) return;

        messageList.innerHTML = "";

        this.context.inbox.forEach((item) => {
            console.log(item);

            let div = document.createElement("div");
            div.className = "hover-actions-trigger chat-contact nav-item";
            div.setAttribute("data-id", item.FriendId);
            div.setAttribute("title", item.FriendName);
            div.setAttribute("role", "tab");
            div.setAttribute("id", "chat-link-0");
            div.setAttribute("data-bs-toggle", "tab");
            div.setAttribute("data-bs-target", "#chat-0");
            div.setAttribute("aria-controls", "chat-0");
            div.setAttribute("aria-selected", "true");


            let divMediaBody = document.createElement('div');
            divMediaBody.className = 'd-md-none d-lg-block';
            divMediaBody.innerHTML = ` 
                     <div class="d-flex p-3">
                       <div class="flex-1 chat-contact-body ms-2 d-md-none d-lg-block">
                          <div class="d-flex justify-content-between">
                            <h6 class="mb-0 chat-contact-title">${item.FriendName}</h6><span class="message-time fs--2">Tue</span>
                          </div>
                          <div class="min-w-0">
                            <div class="chat-contact-content pe-3">${item.LastMessage}</div>
                            <div class="position-absolute bottom-0 end-0 hover-hide">${item.Date}</div>
                          </div>
                        </div>
                      </div>`;

            div.appendChild(divMediaBody);

            messageList.appendChild(div);
        });
    }
} // <-- Added closing brace here.