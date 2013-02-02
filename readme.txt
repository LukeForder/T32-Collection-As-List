When using a ChannelFactory to generate WCF channels, the channel will back properties
as collection interfaces (IList, ICollection etc) will arrays resulting in a
NotSupportedException when attempting to add items to the collection.

By creating a custom implementation of IClientMessageFormatter we gain access to the 
deserialized replies through the DeserializeReply method. Here By working through the reply
object graph a object grag
  Wrapping the standard Message Formatter with a custom implementation of IClientMessageFormatter
and altering the DeserializeReply to check the deserialized replies for properties of type 