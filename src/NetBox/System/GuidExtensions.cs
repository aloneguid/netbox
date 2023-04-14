
namespace System {
    using NetBox;

    static class GuidExtensions {
        public static string ToShortest(this Guid g) {
            return Ascii85.Instance.Encode(g.ToByteArray(), true);
        }
    }
}
