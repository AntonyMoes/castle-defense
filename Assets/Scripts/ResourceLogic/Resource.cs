namespace ResourceLogic {
    public struct Resource {
        public Resource(ResourceType resourceType, int weight) {
            ResourceType = resourceType;
            Weight = weight;
        }

        public readonly ResourceType ResourceType;
        public readonly int Weight;
    }
}
